using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sequential2013.Domain.Models {

   public class BlogPostVModel {

      public BlogPostVModel(int bPostId, string title, string desc, 
                            string excerpt, string extended, 
                            DateTime createDate) {
         BlogPostId = bPostId;
         Title = title;
         Description = desc;
         Excerpt = excerpt;
         ExtendedDescription = extended;
         CreateDate = createDate;
			Tags = new List<TagVModel>();
      }

      public BlogPostVModel() {
			Tags = new List<TagVModel>();
		}

      public int BlogPostId { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
      public string Excerpt { get; set; }
      public string ExtendedDescription { get; set; }
		public string Permalink { get; set; }
		public BookVModel Book { get; set; }
		public List<TagVModel> Tags { get; set; }
      public DateTime CreateDate { get; set; }

		public bool HasTags() { return Tags.Count() > 0; }

      public string FullPostText() {
         return Description + ExtendedDescription;
      }

      public bool HasExtendedDescription() {
         if (ExtendedDescription == null) return false;
         return ExtendedDescription.Length>0;
      }
   }

}