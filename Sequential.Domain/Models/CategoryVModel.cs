namespace Sequential2013.Domain.Models 
{
   public class CategoryVModel {

      public CategoryVModel() { }
      public CategoryVModel(int id, string name, int count) {
         CategoryId = id;
         Name = name;
         Tally = count;
      }

		public bool IsBook { get; set; }
      public int CategoryId { get; set; }
      public string Name { get; set; }
      public int Tally { get; set; }

      public override string ToString() {
         return Name + " (" + Tally + ")";
      }
   }
}