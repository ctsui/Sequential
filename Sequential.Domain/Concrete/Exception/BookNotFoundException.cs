using System;

namespace Sequential2013.Domain.Concrete.Exception
{
   public class BookNotFoundException : ApplicationException
   {
      public BookNotFoundException(string message)
         : base(message) {}
   }
}
