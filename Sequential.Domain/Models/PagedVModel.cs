namespace Sequential2013.Domain.Models 
{
public class PagedVModel {
	public int CurrentPage { get; set; }
	public int PageSize { get; set; }
	public bool HasMorePages { get; set; }
	public string BlogId { get; set; }
	//Do we really need to move these values to the view manually?
	public string Action { get; set; }
	public string Controller { get; set; }
}
} //end namespace