using System.Web.Mvc;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;

namespace Sequential2013.WebUI.Controllers
{
/// <summary>
/// The <code>WebComicController</code> is an extension of the PostController
/// as the typical webcomic page also has a blog. Usually, a blog post is 
/// synced with a comic, but in Sequential this does not have to be the case
/// and is currently implemented with the comic page being independent of the
/// blog article. This is partly because the comic pages are loaded via ajax 
/// calls and partly because steady state I don't imagine having a desire to
/// write an article with each and every page post.
/// 
/// If the comic pages need to be synchronized with a blog article per page
/// then it makes the most sense to disable ajax fetching of the comic pages.
/// This is a TODO item at this point.
/// </summary>
public class WebComicController : PostController
{

	public WebComicController(	ISeqPostsRepository pRep,
										ISeqCategoriesRepository cRep,
										ISeqTagsRepository tRep)
		: base(pRep, cRep, tRep) { }

	/// <summary>
	/// Processes a URI that can reference a page of a comic as well as a specific
	/// blog article. A comic page (image) must be returned, but a specific blog
	/// article need not be named.
	/// </summary>
	/// <param name="comic">The name of the comic should match the <code>SeqBook
	/// </code> UriContext property. The value of this parameter is also used as
	/// the <code>SeqPost</code> BlogId for the purposes of looking up blog posts
	/// related to the comic.</param>
	/// <param name="chapter">A chapter number 1..n</param>
	/// <param name="page">A page number 1..n</param>
	/// <param name="perma">Optional. The permalink string to a blog post.</param>
	/// <param name="id">Optional. Blog post id that must accompany a 
	/// permalink since this is how the system looks up the article.</param>
	/// <returns>A view that can be shown for any comic and/or blog article
	/// request.</returns>
	public ActionResult FullPathEntry(	string comic, int chapter, int page,
													string perma=null, int id = -1)
	{
		ViewResult result;
		//Specific blog identified 
		if (id > 0) {
			result = (ViewResult)base.GetSinglePost(comic,id);
		} else {
			 result = (ViewResult)base.Index(comic);
		}
		WebComicVModel wcvm = new WebComicVModel((BlogHomeVModel) result.Model);
		wcvm.BookName = comic;
		wcvm.ChapterNumber = chapter;
		wcvm.PageNumber = page;
		//wcvm.Controller = "WebComic";
		//wcvm.Action = "FullPathEntry"; //Need PartialView action like TurnPage
		return View("Index",wcvm);
	}

	/// <summary>
	/// Action that generates a partial view for just a comic page. This method is
	/// meant to respond to ajax requests for a comic page.
	/// </summary>
	/// <param name="comic">The name of the comic being viewed. This value must
	/// match a record in the SeqBOok table with a UriContext of the same value.
	/// </param>
	/// <param name="chapter">A chapter number 1..n</param>
	/// <param name="page">A page number 1..n</param>
	/// <returns>A partial view that contains a comic page and navigation 
	/// controls.</returns>
	public ActionResult TurnPage(string comic, int chapter, int page)
	{
		//If No Cookie in HttpRequest then start at the beginning
		WebComicVModel wcvm = new WebComicVModel();
		wcvm.BookName = comic;
		wcvm.ChapterNumber = chapter;
		wcvm.PageNumber = page;
		return PartialView(wcvm);
	}

	/// <summary>
	/// Serves the most recent comic page and blog post. This method was meant
	/// to handle the simplest URI for the webcomic such that a chapter or page
	/// number is not needed. (e.g. localhost/wonderdog)
	/// </summary>
	/// <param name="comic">This value must match a record in the SeqBook table
	/// with a UriContext of the same value.</param>
	/// <returns>Delegates to the result of the FullPathEntry method.</returns>
	public ActionResult MostRecentAll(string comic)
	{
		//TODO: Check that comic is in SeqBook collection. If not then reject.
		//TODO: Read database for lastest comic chapter/page or override with cookie.
		int lastChapter = 1;
		int lastPage = 1;
		//By not providing an article id this method fetches the most recent post.
		return FullPathEntry(comic, lastChapter, lastPage);
	}

} //end class WebComicController
} //end namespace
