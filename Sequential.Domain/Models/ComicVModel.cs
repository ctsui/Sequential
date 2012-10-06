using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sequential2013.Domain.Models {
   public class ComicVModel {

      public string Title { get; set; }
      /// <summary>
      /// True if currently showing the first page.
      /// </summary>
      public bool AtFirst { get; set; }
      /// <summary>
      /// True if currently showing the last page.
      /// </summary>
      public bool AtLast { get; set; }

		/// <summary>
		/// Used with single page layout; otherwise null for vertical page layout.
		/// </summary>
      public BlogPostVModel CurrentPage { get; set; }
		
		/// <summary>
		/// Used with vertical page layout; otherwise null for single page layout.
		/// </summary>
		public List<BlogPostVModel> VerticalPages { get; set; }

		public string UriContext { get; set; }

      public ComicVModel(string title, string uriContext) {
         Title = title;
			UriContext = uriContext;
      }

   }
}