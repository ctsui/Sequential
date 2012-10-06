using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain;

namespace Sequential2013.Domain.Concrete {
   public class SeqTagsRepository : ISeqTagsRepository {
		
		private SeqEFContext db;
		private const int MAX_PAGE_SIZE = 50;

      public SeqTagsRepository(string connectionString) {
			db = new SeqEFContext(connectionString);
      }

		/// <summary>
		/// Get the collection of tags for a post.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <returns></returns>
		public IQueryable<SeqTag> TagsForPost(int postId) {

			IQueryable<SeqTag> postTags = 
				db.SeqTags.Where(t => t.SeqPosts.Any(p => p.PostId == postId));
			return postTags;
		}

		public IQueryable<SeqPost> PostsForTag(SeqTag tag, string sortOrder,
															int pageSize, int pageNum) {

			pageSize = (pageSize > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : pageSize;
			IQueryable<SeqPost> posts;
			switch (sortOrder) {
				case "desc":
					posts = tag.SeqPosts
								  .OrderByDescending(p=>p.CreateDate)
								  .Skip((pageNum - 1) * pageSize)
								  .Take(pageSize)
								  .AsQueryable<SeqPost>();
					break;
				default:
					posts = tag.SeqPosts
								  .OrderBy(p => p.CreateDate)
								  .Skip((pageNum - 1) * pageSize)
								  .Take(pageSize)
								  .AsQueryable<SeqPost>();
					break;
			}
			return posts;
		}

      /// <summary>
      /// Gets the tags (i.e. keywords) for a post as a comma delimited string
      /// for use with MetaWeblog XMLRPC gateway response to GetPost.
      /// </summary>
      /// <param name="postId">The post for which to get keywords.</param>
      /// <returns>Comma delimited string of keywords. Tags with spaces in 
      /// them are quoted first.</returns>
      public string GetKeywords(int postId) {
			List<SeqTag> postTags = TagsForPost(postId).ToList<SeqTag>();
         StringBuilder keywords = new StringBuilder();
         foreach (SeqTag st in postTags) {
            if (st.Name.Contains(' ')) {
               keywords.Append("\"");
               keywords.Append(st.Name);
               keywords.Append("\"");
            }
            else keywords.Append(st.Name);
            keywords.Append(", ");
         }
         if (keywords.Length > 2)
            return keywords.Remove(keywords.Length - 2, 1).ToString();
         else
            return keywords.ToString();
      }

      /// <summary>
      /// Get a tag given a name.
      /// </summary>
      /// <param name="tagName">The name of the tag to find.</param>
      /// <returns>The SeqTag that matches the tagName. In the unlikely event that
      /// there is more than one match this method returns null.</returns>
      public SeqTag GetTag(string tagName, string blogId) {
         try {
            SeqTag theTag = db.SeqTags.Single(t => t.Name == tagName);
            return theTag;
         }
         catch (InvalidOperationException) { return null; }
      }

		public SeqTag GetTag(int id) {
			return db.SeqTags.SingleOrDefault(t => t.TagId == id);
		}
   }
}