using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sequential2013.Domain.Models {
   public class PostsForCategoryVModel : BlogHomeVModel {

      public List<BlogPostVModel> AllPosts { get; set; }
      public string Category { get; set;  }

      public PostsForCategoryVModel(string theCategory) {
         Category = theCategory;
			this.AllCategories = new List<CategoryVModel>();
      }

   }
}