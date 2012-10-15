using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;
using Sequential2013.Domain;

namespace Sequential2013.WebUI.Controllers {
/// <summary>
/// Handles category searches for the blog.
/// </summary>
public class CategoryController : Controller {

   private ISeqPostsRepository postsRep;
	private ISeqCategoriesRepository catRep;
	private ISeqTagsRepository tagRep;
	private int pageSize;
	private string blogId;

   public CategoryController(	ISeqPostsRepository pRep, 
										ISeqCategoriesRepository cRep,
										ISeqTagsRepository tRep) {
      postsRep = pRep;
		catRep = cRep;
		tagRep = tRep;
		pageSize = Convert.ToInt32(ConfigurationManager
								.AppSettings["PageSize"]);
		if (pageSize == 0) pageSize = 1;
		blogId = ConfigurationManager.AppSettings["BlogId"];
   }

	public ActionResult PostsForCategory(string category, string order,
														int pageNum = 1)
	{
		PostsForCategoryVModel pcvm = GetPostsForCategory(	null, category,
																			order, pageNum);
		pcvm.Action = "PostsForCategory";
		return View("PostsForCategory", pcvm);
	}

	/// <summary>
	/// Get the posts for a specific category and return as a partial view.
	/// </summary>
	/// <param name="bid">The blog id.</param>
	/// <param name="category">The category associated with the posts.</param>
	/// <param name="order">desc or asc</param>
	/// <param name="pageNum">The server-side page in multi-page result.</param>
	/// <returns>Partial view for insertion into a div.</returns>
	public ActionResult AjaxPostsForCategory(	string bid, string category,
															string order, int pageNum = 1)
	{
		PostsForCategoryVModel pcvm = GetPostsForCategory(bid, category, order, pageNum);
		pcvm.Action = "AjaxPostsForCategory";
		return PartialView("AjaxPostsForCategory", pcvm);
	}

	private PostsForCategoryVModel GetPostsForCategory(string bid, string category,
		string order, int pageNum=1)
	{
		if (bid == null) bid = blogId;
		IQueryable<SeqPost> catPosts = postsRep.PostsForCategory(
												 bid, category, order, pageSize, pageNum);

		PostsForCategoryVModel pcvm = new PostsForCategoryVModel(category);
		pcvm.BlogId = bid;
		pcvm.Controller = "Category";
		pcvm.AllPosts = VModelFactory.BlogPosts(catPosts);
		pcvm.CurrentPage = pageNum;
		pcvm.PageSize = pageSize;
		pcvm.HasMorePages = pcvm.AllPosts.Count() > 0;
		pcvm.AllCategories = VModelFactory.AllCategories(
									catRep.AllCategories(bid),bid);
		//pcvm.News = VModelFactory.BlogPosts(catRep.RecentNews(5));
		//pcvm.Books = VModelFactory.Books(bookRep.AllBooks());
		return pcvm;
	}
	
} //end class
} //end namespace