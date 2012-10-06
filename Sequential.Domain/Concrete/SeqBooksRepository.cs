using System.Data.Linq;
using System.Linq;
using Sequential.Domain.Abstract;
using Sequential2013.Domain;

namespace Sequential.Domain.Concrete {
	public class SeqBooksRepository :ISeqBooksRepository {

		private Table<SeqBook> readBooks;

		public SeqBooksRepository(string connectionString) {
			EntitiesDataContext EDCRead = new EntitiesDataContext(connectionString);
			readBooks = EDCRead.GetTable<SeqBook>();
		}

		public IQueryable<SeqBook> AllBooks() {
			return readBooks;
		}

		public SeqBook GetBook(string title) {
			return readBooks.SingleOrDefault(b => b.Title == title);
		}

		public SeqBook BookUriContext(string context) {
			return readBooks.SingleOrDefault(b => b.UriContext == context);
		}
	}
}
