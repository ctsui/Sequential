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
		private ISeqTagsRepository tagRep;
		private ISeqCategoriesRepository catRep;

		public TagController(ISeqPostsRepository pRep,
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

		public ActionResult PostsForTag(	int tagId, string bid, int pageNum,
													string order) 
		{
			
			TagHomeVModel thvm = GetPostsForTag(tagId, bid, pageNum, order);
			if (thvm == null) return View("NoTagsFound");
			thvm.Action = "PostsForTag";
			thvm.AllCategories = VModelFactory.AllCategories(
										catRep.AllCategories(bid), bid);
			return View("PostsForTag",thvm);
		}

		public ActionResult AjaxPostsForTag(int tagId, string bid, int pageNum,
														string order)
		{
			TagHomeVModel thvm = GetPostsForTag(tagId, bid, pageNum, order);
			//if (thvm == null) return PartialView("NoTagsFound");
			thvm.Action = "AjaxPostsForTag";
			return PartialView(thvm);
		}

		private TagHomeVModel GetPostsForTag(	int tagId, string bid,
															int pageNum, string order)
		{
			if (bid == null) bid = blogId;
			SeqTag tag = tagRep.GetTag(tagId, bid);
			//if (tag == null) return null;
			IQueryable<SeqPost> tagPosts = tagRep.PostsForTag(
				tag, order, pageSize, pageNum);

			TagHomeVModel thvm = new TagHomeVModel(tagId);
			thvm.Tag = tag.Name;
			thvm.BlogId = bid;
			thvm.RecentPosts= VModelFactory.BlogPosts(tagPosts);
			thvm.Controller = "Tag";
			thvm.CurrentPage = pageNum;
			thvm.PageSize = pageSize;
			thvm.HasMorePages = thvm.RecentPosts.Count() > 0;
			//thvm.News = VModelFactory.BlogPosts(catRep.RecentNews(5));
			//thvm.Books = VModelFactory.Books(bookRep.AllBooks());
			return thvm;
		}
	}
}
