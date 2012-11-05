using System.Linq;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Concrete.Exception;

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

		public SeqBook BookUriContext(string context)
      {
			SeqBook sb = db.SeqBooks.SingleOrDefault(b => b.UriContext == context);
         if (sb == null) 
            throw new BookNotFoundException( "The book \""+context+"\" was "+
                                             "not found.");
         return sb;
		}

		public SeqChapter GetChapter(SeqBook book, int chapterNum) 
		{
         SeqChapter sc = book.SeqChapters.SingleOrDefault(
                           c => c.ChapterNum == chapterNum);
         if (sc == null)
            throw new ChapterNotFoundException("Chapter \"" + chapterNum + 
               "\" was not found for the book \"" + book.Title + "\"");
         return sc;
		}
      
      public IQueryable<SeqPage> GetRecentPages(string book, int numPages=10)
      {
         return db.SeqPages.Where(p => p.SeqChapter.SeqBook.UriContext==book)
                           .OrderByDescending(p => p.PubDate)
                           .Take(numPages);
      }
	}
}
