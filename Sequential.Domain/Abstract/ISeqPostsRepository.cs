using System.Linq;
using Sequential2013.Domain.Models;

namespace Sequential2013.Domain.Abstract {
   public interface ISeqPostsRepository {
		
      IQueryable<SeqPost> GetPostPage(int pageSize, int pageNum, 
			string blogid=null);
      
		IQueryable<SeqPost> PostsForCategory(string blogId, 
			string category, string sortOrder, int pageSize, int pageNum);

		//IQueryable<SeqPost> PostsForBook(string blogId,
		//   string bookTitle, string sortOrder, int pageSize, int pageNum);

		string SavePost(string blogid, Post p, bool publish);
		bool UpdatePost(Post p, bool publish);
      IQueryable<SeqPost> GetPost(int id);
   }
}
