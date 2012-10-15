namespace Sequential2013.Domain.Models
{
public class TagHomeVModel : BlogHomeVModel 
{
	public string Tag { get; set; }
	public int Id { get; set; }

	public TagHomeVModel(int id)
	{
		Id = id;
	}
}
}