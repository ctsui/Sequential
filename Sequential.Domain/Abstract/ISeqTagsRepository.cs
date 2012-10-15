using System.Linq;

namespace Sequential2013.Domain.Abstract {

   public interface ISeqTagsRepository {
      SeqTag GetTag(string tagName, string blogId);
      string GetKeywords(int postId);
		IQueryable<SeqTag> TagsForPost(int postId);
		SeqTag GetTag(int id, string blogId);
		IQueryable<SeqPost> PostsForTag(SeqTag tag, string sortOrder,
			int pageSize, int pageNum);
   }
}
