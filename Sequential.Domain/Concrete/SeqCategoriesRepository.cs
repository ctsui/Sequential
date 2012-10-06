using System.Data.Linq;
using System.Linq;
using Sequential2013.Domain;
using Sequential2013.Domain.Abstract;

namespace Sequential2013.Domain.Concrete {
public class SeqCategoriesRepository : ISeqCategoriesRepository 
{
	private SeqEFContext db;
	private const string NEWS = "News";

   public SeqCategoriesRepository(string connectionString) 
	{
		db = new SeqEFContext(connectionString);
   }

   /// <summary>
   /// Get all categories with at least one post entry in descending order
   /// by category name ascending.
   /// </summary>
   public IQueryable<SeqCategory> AllCategories(string blogId)
	{
      return db.SeqCategories.Where(c => c.Tally > 0 && c.BlogId==blogId)
                              .OrderBy(c => c.Name);
   } 

   /// <summary>
   /// Get a category by name.
   /// </summary>
   /// <param name="name">Name of catetory</param>
   /// <returns>The corresponding SeqCategory or null.</returns>
   public SeqCategory GetCategory(string name)
	{
      return db.SeqCategories.SingleOrDefault(c => c.Name == name);
   }

	public IQueryable<SeqPost> RecentNews(int num) {
		SeqCategory cat = db.SeqCategories.SingleOrDefault(c => c.Name == NEWS);
		if (cat == null) return null;
		return cat.SeqPosts.AsQueryable();
	}

}
}
