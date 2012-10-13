using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using Sequential2013.Domain;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;

namespace Sequential2013.WebUI.Controllers
{
/// <summary>
/// Controller that handles fetching blog posts and categories.
/// Note that the term page as used here refers to server side pages of blog
/// posts and not a comic page.
/// </summary>
public class PostController : Controller
{
	private string blogId;
	private int pageSize;
	private ISeqPostsRepository postsRep;
	private ISeqCategoriesRepository catRep;
	private ISeqTagsRepository tagRep;

	public PostController(	ISeqPostsRepository pRep,
									ISeqCategoriesRepository cRep,
									ISeqTagsRepository tRep)
	{
		postsRep = pRep;
		catRep = cRep;
		tagRep = tRep;
		pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
		if (pageSize == 0)
			pageSize = 1;
		blogId = ConfigurationManager.AppSettings["BlogId"];
	}

	public virtual ActionResult Index(string blogId = null)
	{
		return PostPage(blogId, 1);
	}

	public ActionResult PostPage(string bid = null, int pageNum = 1)
	{
		if (pageNum < 1) pageNum = 1;
		if (bid == null) bid = blogId;
		BlogHomeVModel bhvm = new BlogHomeVModel();
		bhvm.BlogId = bid;
		IQueryable<SeqPost> results = postsRep.GetPostPage(pageSize, pageNum, bid);
		bhvm.RecentPosts = VModelFactory.BlogPosts(results);
		bhvm.AllCategories = VModelFactory.AllCategories(catRep.AllCategories(bid),bid);
		bhvm.CurrentPage = pageNum;
		bhvm.PageSize = pageSize;
		bhvm.HasMorePages = bhvm.RecentPosts.Count() > 0;
		bhvm.Controller = "post";
		bhvm.Action = "PostPage";
		return View("Index", bhvm);
	}

	/// <summary>
	/// Fetch a page of blog posts and returns a partial view. 
	/// </summary>
	/// <param name="bid">Blog id</param>
	/// <param name="pageNum">The requested page of blog posts.</param>
	/// <returns>A partial view containing a page of blog posts.</returns>
	public ActionResult AjaxPostPage(string bid = null, int pageNum = 1)
	{
		if (pageNum < 1) pageNum = 1;
		if (bid == null) bid = blogId;
		BlogHomeVModel bhvm = new BlogHomeVModel();
		bhvm.BlogId = bid;
		IQueryable<SeqPost> results = postsRep.GetPostPage(pageSize, pageNum, bid);
		bhvm.RecentPosts = VModelFactory.BlogPosts(results);
		bhvm.CurrentPage = pageNum;
		bhvm.PageSize = pageSize;
		bhvm.HasMorePages = bhvm.RecentPosts.Count() > 0;
		bhvm.Controller = "post";
		bhvm.Action = "PostPage";
		return PartialView(bhvm);
	}

	[ValidateInput(false)]
	public ActionResult GetSinglePost(string bid, int id)
	{
		if (bid == null)
			bid = blogId;
		IQueryable<SeqPost> sp = postsRep.GetPost(id);
		BlogHomeVModel bhvm = new BlogHomeVModel();
		bhvm.BlogId = bid;
		bhvm.RecentPosts = VModelFactory.BlogPosts(sp);
		bhvm.AllCategories = VModelFactory.AllCategories(
										catRep.AllCategories(bid),bid);
		return View(bhvm);
	}
	/// <summary>
	/// Generates a RSS feed of the 10 most recent posts.
	/// </summary>
	/// <returns>RSS 2.0 feed</returns>
	public ContentResult RssFeed(string feedName = null)
	{
		Uri u = Request.RequestContext.HttpContext.Request.Url;
		string applicationPath = Request.RequestContext
													.HttpContext.Request
													.ApplicationPath;
		string linkUri = u.Scheme + "://" + u.Host + applicationPath;
		List<SeqPost> recentPosts = postsRep.GetPostPage(10, 1, blogId).ToList<SeqPost>();
		string lastBuildDate = recentPosts.First().CreateDate.ToString();
		string encoding = Response.ContentEncoding.WebName;
		if (feedName == null)
			feedName = "Sputnik Comics";
		XDocument rss = new XDocument(
			new XDeclaration("1.0", encoding, "yes"),
			new XElement("rss", new XAttribute("version", "2.0"),
				new XElement("channel", new XElement("title", feedName),
					new XElement("link", linkUri),
					new XElement("language", "en-us"),
					new XElement("lastBuildDate", lastBuildDate),
					new XElement("generator", "Sequential2013"),
					from post in recentPosts
					select new XElement("item",
						new XElement("title", post.Title),
						new XElement("description",
							(post.Excerpt == "") ? post.Description : post.Excerpt),
						new XElement("link", linkUri + "/read/" + post.PostId + "/" + post.Permalink),
						new XElement("pubDate", post.CreateDate.ToString()),
						new XElement("category", post.SeqCategory.Name)
					) //XElement item
			) //XElement channel
		)); //XDocument rss
		return Content(rss.ToString(), "application/rss+xml");
	}
} //end class PostController
} //end namespace