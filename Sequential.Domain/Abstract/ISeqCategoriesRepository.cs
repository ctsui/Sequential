using System.Linq;

namespace Sequential2013.Domain.Abstract {
   public interface ISeqCategoriesRepository {
		IQueryable<SeqCategory> AllCategories(string blogId);
      SeqCategory GetCategory(string name);
		IQueryable<SeqPost> RecentNews(int num);
   }
}
