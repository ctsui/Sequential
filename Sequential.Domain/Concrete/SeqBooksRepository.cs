using System.Data.Linq;
using System.Linq;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain;

namespace Sequential2013.Domain.Concrete {
	public class SeqBooksRepository :ISeqBooksRepository {

		private SeqEFContext db;

		public SeqBooksRepository(string connectionString) {
			db = new SeqEFContext(connectionString);
		}

		public IQueryable<SeqBook> AllBooks() {
			return db.SeqBooks;
		}

		public SeqBook GetBook(string title) {
			return db.SeqBooks.SingleOrDefault(b => b.Title == title);
		}

		public SeqBook BookUriContext(string context) {
			return db.SeqBooks.SingleOrDefault(b => b.UriContext == context);
		}

		public SeqChapter GetChapter(string context, int chapterNum)
		{
			SeqBook book = BookUriContext(context);
			return book.SeqChapters.SingleOrDefault(c => c.ChapterNum == chapterNum);
		}
	}
}
