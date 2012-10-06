namespace Sequential2013.Domain.Models
{
public class WebComicVModel : BlogHomeVModel {

	public WebComicVModel(BlogHomeVModel bhvm) 
	{
		this.RecentPosts = bhvm.RecentPosts;
		this.AllCategories = bhvm.AllCategories;
	}

	public int PageNumber { get; set; }
	public int ChapterNumber { get; set; }
	public string BookName { get; set; }
	public int ChapterPageCount { get; set; }

	public string GetFeaturedPost()
	{ return RecentPosts[0].Permalink; }

	public int GetFeaturedPostId()
	{ return RecentPosts[0].BlogPostId; }

	public string FileName()
	{ return "EdenC" + ChapterNumber + "P" + PageNumber; }

}
} //end namespace
