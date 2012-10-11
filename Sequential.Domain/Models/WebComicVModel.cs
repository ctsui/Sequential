using System.Collections.Generic;
namespace Sequential2013.Domain.Models
{
/// <summary>
/// The <code>WebComicVModel</code> encapsulates properties needed by view(s)
/// to display a webcomic page along with any associated blog post(s).
/// </summary>
public class WebComicVModel : BlogHomeVModel {

    private const string NO_RECENT_POSTS = "";

    public WebComicVModel() 
    { 
        RecentPosts = new List<BlogPostVModel>();
        AllCategories = new List<CategoryVModel>();
    }

	/// <summary>
	/// Create a new <code>WebComicVModel</code> that clones the properties of
	/// its superclass.
	/// </summary>
	/// <param name="bhvm">The parent <code>BlogHomeVModel</code>to clone.
	/// </param>
	public WebComicVModel(BlogHomeVModel bhvm) 
	{
		this.RecentPosts = bhvm.RecentPosts;
		this.AllCategories = bhvm.AllCategories;
		this.Action = bhvm.Action;
		this.Controller = bhvm.Controller;
		this.CurrentPage = bhvm.CurrentPage;
		this.HasMorePages = bhvm.HasMorePages;
		this.PageSize = bhvm.PageSize;
		this.BlogId = bhvm.BlogId;
		bhvm = null;
	}

	public int PageNumber { get; set; }
	public int ChapterNumber { get; set; }
	public string BookName { get; set; }
	public int ChapterPageCount { get; set; }

    /// <summary>
    /// The Featured Post is the most recent post that appears at the top of
    /// a list of posts. When bookmarking of the comic page happens it is 
    /// assumed that the featured post is what is of interest (aside from the
    /// comic page itself).
    /// </summary>
    /// <returns>The permalink to the featured post or an empty string if the
    /// list of posts is empty (e.g. on system initi).</returns>
	public string GetFeaturedPost()
	{
        if (RecentPosts.Count > 0) return RecentPosts[0].Permalink;
        else return NO_RECENT_POSTS;
    }

	public int GetFeaturedPostId()
	{
        if (RecentPosts.Count > 0) return RecentPosts[0].BlogPostId;
        else return 0;
    }

	public string FileName()
	{ 
		return BookName+"C" + ChapterNumber + "P" + PageNumber; 
	}

}
} //end namespace
