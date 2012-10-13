using System;
using System.Collections.Generic;
using System.Linq;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;
using Sequential2013.Domain.Utilities;

namespace Sequential2013.Domain.Concrete {

public class SeqPostsRepository : ISeqPostsRepository {

	private SeqEFContext db;

   //private Table<TagPostJoin> tagPostJoinTable;
   private char[] FORBIDDEN_CHARS = { '"', '\'','<','>','&','.',';' };
   private const string UNCATEGORIZED = "Uncategorized";
	private const int MAX_PAGE_SIZE = 50;

   /// <summary>
   /// Create a SeqPosts table access helper class.
   /// </summary>
   /// <param name="connectionString">Connection string to the application
   /// database.</param>
   public SeqPostsRepository(string connectionString)
	{
		db = new SeqEFContext(connectionString);			
   }

	/// <summary>
	/// Get a page of blog posts ordered from most recent create date.
	/// </summary>
	/// <param name="pageSize">The number of records per page.</param>
	/// <param name="pageNum">The page number.</param>
	/// <param name="blogid">The blog identifier for the records.</param>
	/// <returns>A collection of <code>SeqPost</code>s.</returns>
   public IQueryable<SeqPost> GetPostPage(int pageSize, int pageNum, 
														string blogid)
	{
		pageSize = (pageSize > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : pageSize;
		return db.SeqPosts.Where(sp=>sp.BlogId==blogid)
								.OrderByDescending(sp => sp.CreateDate)
								.Skip((pageNum - 1) * pageSize)
								.Take(pageSize); 
   }

   public IQueryable<SeqPost> PostsForCategory(	string blogId, 
																string category, 
																string sortOrder, 
																int pageSize, int pageNum)
	{
			
		pageSize = (pageSize > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : pageSize;
      switch (sortOrder) {
         case "desc":
				return db.SeqPosts.Where(sp => sp.SeqCategory.Name == category 
													&& sp.BlogId == blogId)
										.OrderByDescending(sp => sp.PostId)
										.Skip((pageNum - 1) * pageSize)
										.Take(pageSize);
         default:
				return db.SeqPosts.Where(sp => sp.SeqCategory.Name == category 
													&& sp.BlogId == blogId)
										.OrderBy(sp => sp.PostId)
										.Skip((pageNum-1)*pageSize)
										.Take(pageSize);
      }
   }

   /// <summary>
   /// Get a single post by its primary key. 
   /// </summary>
   /// <param name="id">The SeqPost.PostId to find.</param>
   /// <returns>
   /// A corresponding SeqPost matching the id or the default.
   /// </returns>
   public IQueryable<SeqPost> GetPost(int id) {
		IQueryable<SeqPost> result = db.SeqPosts.Where(sp => sp.PostId == id);
      return result;
   }

	////TODO: Throw some kind of exception on failure.
	/// <summary>
	/// Save a post from Windows Live Writer to the database.
	/// </summary>
	/// <param name="blogid">The blog identifier string.</param>
	/// <param name="p">The post content and metadata</param>
	/// <param name="publish">Whether this is published or not. Currently 
	/// this flag is not supported by the views.</param>
	/// <returns>The post id number.</returns>
	public string SavePost(string blogid, Post p, bool publish)
	{
		SeqPost sp = new SeqPost();
		sp.BlogId = blogid;
		sp.Title = p.title;
		sp.Description = p.description;
		sp.ExtendedText = p.mt_text_more;
		sp.Excerpt = p.mt_excerpt;
		/* SQLServer can not deal with year < 1753, but dateCreated can be
		   year 1 when WLW2011 post date is not set. For practical 
		   purposes we use 1970 as the minimum date. */
		if (p.dateCreated.Year < 1970)
		   sp.CreateDate = DateTime.Now.ToUniversalTime();
		else
		   sp.CreateDate = p.dateCreated;

		sp.ModifiedDate = DateTime.Now.ToUniversalTime();
		sp.Published = publish;
		sp.Permalink = Permalink.Generate(sp.Title);
		UpdateCategories(p.categories.Length>0 ? p.categories[0] : "",sp);
		UpdateTags(p.mt_keywords, sp);
		db.SeqPosts.AddObject(sp);
		db.SaveChanges();
		return sp.PostId.ToString();
	}

	public bool UpdatePost(Post p, bool publish) 
	{
		SeqPost sp = GetPost(Convert.ToInt32(p.postid)).SingleOrDefault<SeqPost>();
		string oldCatName = sp.SeqCategory.Name;
		sp.Title = p.title;
		sp.Description = p.description;
		sp.ExtendedText = p.mt_text_more;
		sp.Excerpt = p.mt_excerpt;
		/* Should this be set to p.CreateDate instead? */
		sp.ModifiedDate = DateTime.Now.ToUniversalTime();
		sp.Published = publish;
		sp.Permalink = Permalink.Generate(sp.Title);
		UpdateCategories(p.categories.Length > 0 ? p.categories[0] : "",sp);
		/*UpdateBooks(sp,oldcat,newcat) run in UpdateCategories due to the
		   possibility of recategorization of a post. */
	   UpdateTags(p.mt_keywords, sp);
		db.SaveChanges();
		return true;
	}

	///// <summary>
	///// Updates SeqBooks table with new or removed posts. A book is a 
	///// collection of posts without regard to chronology of the posts. A book
	///// can be navigated with different views than that used for regular 
	///// posts. Currently, the book title much exactly match a category for 
	///// new posts to be addable to the book through WLW2011. Note that the
	///// book is retrieved through the SeqBook.UriContext in order to provide
	///// shorter URLs and avoid special characters which get encoded ugly.
	///// </summary>
	///// <param name="sp"></param>
	///// <param name="oldCategory"></param>
	///// <param name="newCategory"></param>
	//private void UpdateBooks(SeqPost sp, SeqCategory oldCategory=null, 
	//                         SeqCategory newCategory=null) {
	//   /* Change of categories so remove post from old category/book */
	//   if ((oldCategory!=null && newCategory!=null) &&
	//       (oldCategory.Name!= newCategory.Name)) {
	//      SeqBook oldBook = booksTable.SingleOrDefault(
	//         b => b.Title ==oldCategory.Name);
	//      if (oldBook != null) {
	//         oldBook.SeqPosts.Remove(sp);
	//         oldBook.PageCount = oldBook.SeqPosts.Count();
	//         booksTable.Context.Refresh(RefreshMode.KeepCurrentValues, 
	//                                    oldBook);
	//      }
	//   }
	//   /* Add post to existing book if post is in a category designated book. */
	//   bool catIsBook = booksTable.Any(b => b.Title == sp.SeqCategory.Name);
	//   if (catIsBook) {
	//      SeqBook book = booksTable.Single(b => b.Title == sp.SeqCategory.Name);
	//      book.SeqPosts.Add(sp);
	//      book.PageCount=book.SeqPosts.Count();
	//      booksTable.Context.Refresh(RefreshMode.KeepCurrentValues, book);
	//   }	
	//}
		
	private void UpdateTags(string tags, SeqPost sp)
	{
	   /* WLW2011 will send an empty string if no tags. */
	   if (tags == null || tags=="") return;
	   string[] allTags = tags.Split(',');
	   /* Simple cleanse */
	   for (int i = 0; i < allTags.Length; i++) {
	      allTags[i] = allTags[i].Trim()
	                             .TrimEnd(FORBIDDEN_CHARS)
	                             .TrimStart(FORBIDDEN_CHARS);
	   }
		List<SeqTag> postTags = sp.SeqTags.ToList<SeqTag>();
	   /* Tallys are recomputed for any tag removed and if
	      the tally is zero then the tag itself is removed. */
	   foreach (SeqTag st in postTags) {
			/* If none of the submitted tags match the current tag then we know it
			   has been removed since WLW2011 sends the complete list of tags for
			   every transaction. */
	      if (!allTags.Any(at => at==st.Name))
			{
				sp.SeqTags.Remove(st); //Delete tag ref from post
				st.SeqPosts.Remove(sp); //Delete post ref from tag
				st.Tally = st.SeqPosts.Count();
				if (st.Tally == 0) db.SeqTags.DeleteObject(st); //Sweep db of empties
	      }
	   }
	   for (int i=0; i<allTags.Length; i++) {
	      SeqTag dbTag;
	      /* Slow...has to make allTags.Length queries to the db. */
			string name = allTags[i]; //Array elements not supported by EF4?!
			if (!db.SeqTags.Any(st => st.Name == name))
			{
	         dbTag = new SeqTag();
	         dbTag.BlogId = sp.BlogId;
	         dbTag.Name = allTags[i];
	         dbTag.LastUpdated = DateTime.Now.ToUniversalTime();
				sp.SeqTags.Add(dbTag);
				dbTag.SeqPosts.Add(sp);
	      }
	      /* Re-use existing tag from SeqTags table */
	      else 
			{
	         dbTag = db.SeqTags.Single(	t => t.Name == name
													&& t.BlogId==sp.BlogId);
	         dbTag.LastUpdated = DateTime.Now.ToUniversalTime();
				dbTag.SeqPosts.Add(sp);
				sp.SeqTags.Add(dbTag);
	      } //end else			
			dbTag.Tally = dbTag.SeqPosts.Count();
	   } //end for
	}

	/// <summary>
	/// Update the categories table with every new or modified post.
	/// </summary>
	/// <param name="catName">
	/// The category name submitted with the post.
	/// </param>
	/// <param name="sp">The post to be persisted.</param>
	private void UpdateCategories(string catName, SeqPost sp) 
	{
		/* A post might not send any category, an existing category, or a new
		   category. In the case of null or empty category we file the post
		   in a category called "None". */
		SeqCategory oldCategory = sp.SeqCategory;
		SeqCategory theCategory = GetUncategorizedCategory();
		if (catName.Length > 0) {
		   theCategory = db.SeqCategories.SingleOrDefault(
									d => d.Name == catName && d.BlogId==sp.BlogId);
		   /* New category was submitted with post */
		   if (theCategory == null) {
		      theCategory = new SeqCategory();
		      theCategory.BlogId = sp.BlogId;
		      theCategory.Name = catName;
		      theCategory.LastUpdated = DateTime.Now.ToUniversalTime();
		      theCategory.Tally = 0;
				sp.SeqCategory = theCategory;         		         
		   }
		   /* Found old category */
		   else {
		      theCategory.LastUpdated = DateTime.Now.ToUniversalTime();
				sp.SeqCategory = theCategory;
		   }
		}
		/* No post category submitted. File under "None" */
		else {
		   /* "None" is not in the database. */
		   if (theCategory == null) {
		      theCategory = new SeqCategory();
		      theCategory.BlogId = sp.BlogId;
		      theCategory.Name = UNCATEGORIZED;
		      theCategory.LastUpdated = DateTime.Now.ToUniversalTime();
		      theCategory.Tally = 0;
				sp.SeqCategory = theCategory;
		   }
		   /* "None" category exists */
		   else {
		      theCategory.LastUpdated = DateTime.Now.ToUniversalTime();
				sp.SeqCategory = theCategory;
		   }
		}
		/* Switching categories - decrement the old category's tally */
		if (oldCategory!=null && (oldCategory.Name != theCategory.Name)) {
		   oldCategory.Tally--;
			oldCategory.SeqPosts.Remove(sp);
		}
		theCategory.Tally = theCategory.SeqPosts.Count();
	}

   /// <summary>
   /// Get the "None" category for uncategorized posts. If it does not yet
   /// exist in the database then create it.
   /// </summary>
   /// <returns>The SeqCategory representing the "None" category or null
   /// if it is not in the database.</returns>
   private SeqCategory GetUncategorizedCategory() {
      SeqCategory cat;
      cat = db.SeqCategories.SingleOrDefault(d => d.Name == UNCATEGORIZED);
      return cat;
   }

} //end class
} //end namespace