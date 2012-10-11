using System.Collections.Generic;

namespace Sequential2013.Domain.Models 
{
public class BlogHomeVModel : PagedVModel {

   public List<BlogPostVModel> RecentPosts { get; set; }
   public List<CategoryVModel> AllCategories { get; set; }
}
} //end namespace