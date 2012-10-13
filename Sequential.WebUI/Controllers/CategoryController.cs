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

   public CategoryController(ISeqPostsRepository pRep, 
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
		IQueryable<SeqPost> catPosts =
			postsRep.PostsForCategory(blogId, category, order, pageSize, pageNum);

		PostsForCategoryVModel pcvm = new PostsForCategoryVModel(category);
		pcvm.AllPosts = VModelFactory.BlogPosts(catPosts);
		pcvm.Controller = "category";
		pcvm.Action = "PostsForCategory";
		pcvm.CurrentPage = pageNum;
		pcvm.PageSize = pageSize;
		pcvm.HasMorePages = pcvm.AllPosts.Count() > 0;
		pcvm.AllCategories = VModelFactory.AllCategories(
										catRep.AllCategories(blogId), blogId);
		//pcvm.News = VModelFactory.BlogPosts(catRep.RecentNews(5));
		//pcvm.Books = VModelFactory.Books(bookRep.AllBooks());
		return View("PostsForCategory", pcvm);
	}

	public ActionResult AjaxPostsForCategory(	string bid, string category, 
															string order, int pageNum=1)
	{
		IQueryable<SeqPost> catPosts = postsRep.PostsForCategory(bid, category,
			order, pageSize, pageNum);
		PostsForCategoryVModel pcvm = new PostsForCategoryVModel(category);
		pcvm.AllPosts = VModelFactory.BlogPosts(catPosts);
		pcvm.CurrentPage = pageNum;
		pcvm.PageSize = pageSize;
		pcvm.HasMorePages = pcvm.AllPosts.Count() > 0;
		pcvm.AllCategories = VModelFactory.AllCategories(
										catRep.AllCategories(bid),bid);
		pcvm.BlogId = bid;
		return View("AjaxPostsForCategory", pcvm);
	}
}
}
