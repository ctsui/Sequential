using System;

namespace Sequential2013.Domain.Concrete.Exception
{
   public class ChapterNotFoundException : ApplicationException
   {
      public ChapterNotFoundException(string message)
         : base(message) {}
   }
}
