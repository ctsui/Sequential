using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Sequential2013.Domain;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;

namespace Sequential2013.WebUI.Controllers {

	public class TagController : Controller {
		private string blogId;
		private int pageSize;
		private ISeqPostsRepository postsRep;
		private ISeqBooksRepository bookRep;
		private ISeqTagsRepository tagRep;
		private ISeqCategoriesRepository catRep;

		public TagController(ISeqPostsRepository pRep,
									ISeqBooksRepository bRep,
									ISeqCategoriesRepository cRep,
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

		public ActionResult PostsForTag(int tagId, int pageNum, string order) {
			SeqTag tag = tagRep.GetTag(tagId);
			if (tag == null) return View("NoTagsFound");

			IQueryable<SeqPost> tagPosts = tagRep.PostsForTag(
				tag, order, pageSize, pageNum);
			TagHomeVModel thvm = new TagHomeVModel();
			thvm.Tag = tag.Name;
			thvm.RecentPosts = VModelFactory.BlogPosts(tagPosts);
			thvm.Controller = "tag";
			thvm.Action = "PostsForTag";
			thvm.CurrentPage = pageNum;
			thvm.PageSize = pageSize;
			thvm.HasMorePages = thvm.RecentPosts.Count() > 0;
			thvm.AllCategories = VModelFactory.AllCategories(
										catRep.AllCategories(blogId));
			//thvm.News = VModelFactory.BlogPosts(catRep.RecentNews(5));
			//thvm.Books = VModelFactory.Books(bookRep.AllBooks());
			return View("PostsForTag",thvm);
		}

	}
}
