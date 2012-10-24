using System.Linq;

namespace Sequential2013.Domain.Abstract {
	public interface ISeqBooksRepository {
		
		IQueryable<SeqBook> AllBooks();
		SeqBook GetBook(string title);
		SeqBook BookUriContext(string context);
		SeqChapter GetChapter(string context, int chapterNum);
	}
}
