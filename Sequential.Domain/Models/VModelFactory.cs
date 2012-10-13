using System.Collections.Generic;
using System.Linq;
using Sequential2013.Domain;
using System;

namespace Sequential2013.Domain.Models {
	/// <summary>
	/// Creates various view model DTOs
	/// </summary>
	public static class VModelFactory {

		public static List<BlogPostVModel> BlogPosts(IQueryable<SeqPost> result) {

			List<BlogPostVModel> blogList = new List<BlogPostVModel>();
			BlogPostVModel post;
			if (result == null) return blogList;
			foreach (SeqPost sp in result) {
				post = new BlogPostVModel();
				post.BlogPostId = sp.PostId;
				post.Title = sp.Title;
				post.Description = sp.Description;
				post.ExtendedDescription = sp.ExtendedText;
				post.Excerpt = sp.Excerpt;
				post.CreateDate = sp.CreateDate;
				post.Permalink = sp.Permalink;
				foreach (SeqTag st in sp.SeqTags) {
				   TagVModel tvm = new TagVModel();
				   tvm.BlogId = st.BlogId;
				   tvm.TagId = st.TagId;
				   tvm.Name = st.Name;
				   post.Tags.Add(tvm);
				}
				blogList.Add(post);
			}
			return blogList;
		}

		public static List<CategoryVModel> AllCategories(
			IQueryable<SeqCategory> result, string blogId) {

			List<CategoryVModel> categoryList = new List<CategoryVModel>();
			CategoryVModel cat;
			foreach (SeqCategory sc in result) {
				cat = new CategoryVModel(	blogId, sc.CategoryId, sc.Name,
													(int)sc.Tally);
				categoryList.Add(cat);
			}
			return categoryList;
		}

		public static List<BookVModel> Books(IQueryable<SeqBook> result) {
			List<BookVModel> bookList = new List<BookVModel>();
			BookVModel book;
			foreach (SeqBook b in result) {
				book = new BookVModel();
				book.Title = b.Title;
				book.PageCount = b.PageCount;
				book.Status = (BookStatus) b.Status;
				bookList.Add(book);
			}
			return bookList;
		}

	} //end class
}