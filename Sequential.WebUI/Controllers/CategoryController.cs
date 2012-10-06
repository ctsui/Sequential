using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;
using Sequential2013.Domain;

namespace Sequential2013.WebUI.Controllers {
   public class CategoryController : Controller {

      private ISeqPostsRepository postsRep;
		private ISeqCategoriesRepository catRep;
		private ISeqBooksRepository bookRep;
		private ISeqTagsRepository tagRep;
		private int pageSize;
		private string blogId;

      public CategoryController(ISeqPostsRepository pRep, 
										  ISeqCategoriesRepository cRep,
										  ISeqBooksRepository bRep,
										  ISeqTagsRepository tRep) {
         postsRep = pRep;
			catRep = cRep;
			tagRep = tRep;
			bookRep = bRep;
			pageSize = Convert.ToInt32(ConfigurationManager
									.AppSettings["PageSize"]);
			if (pageSize == 0) pageSize = 1;
			blogId = ConfigurationManager.AppSettings["BlogId"];
      }

      public ActionResult PostsForCategory(string category, string order,
														 int pageNum=1) {
         /* If category is book then do special display. There should be an 
			   option to flag a category as a book so that we can avoid the
			   redirect and possibly show books as a separate comics list. In which case
			   a comic will never be handled in this method so no redirect necessary. 
			*/
         if (bookRep.AllBooks().Any(b=>b.Title==category))
            return RedirectToRoute("SinglePageComic", new { 
               action="SinglePage",
               order=order,
               category = category
            });
         
			IQueryable<SeqPost> catPosts = 
            postsRep.PostsForCategory(blogId,category, order, pageSize, pageNum);

         PostsForCategoryVModel pcvm = new PostsForCategoryVModel(category);
			pcvm.AllPosts = VModelFactory.BlogPosts(catPosts);
			pcvm.Controller = "category";
			pcvm.Action = "PostsForCategory";
			pcvm.CurrentPage = pageNum;
			pcvm.PageSize = pageSize;
			pcvm.HasMorePages = pcvm.AllPosts.Count() > 0;
			pcvm.AllCategories = VModelFactory.AllCategories(
							catRep.AllCategories(blogId));
			pcvm.News = VModelFactory.BlogPosts(catRep.RecentNews(5));
			//pcvm.Books = VModelFactory.Books(bookRep.AllBooks());
         return View("PostsForCategory",pcvm);
      }


   }
}
