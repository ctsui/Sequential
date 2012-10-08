using System.Web.Mvc;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;

namespace Sequential2013.WebUI.Controllers
{
/// <summary>
/// The WebComicController is an extension of the PostController as the typical
/// webcomic page also has a blog. Usually, a blog post is synced with a comic,
/// but in Sequential this does not have to be the case.
/// </summary>
public class WebComicController : PostController {

	public WebComicController(	ISeqPostsRepository pRep,
										ISeqCategoriesRepository cRep,
										ISeqTagsRepository tRep)
										:base(pRep, cRep, tRep) {}

    //TODO: Rename this ComicInit or something indicating that it's only prepping the page once.

	public ActionResult Comic(string comic, int chapter, int page, int id=-1) 
	{
		//If chapter/page/id equals -1 then localhost/sequential/eden route which 
		//means show latest comic and blog.
		if (chapter == -1 && page == -1 && id == -1) 
		{ }
        //return base.Index(comic);
        ViewResult result = (ViewResult) base.Index(comic);
		//TODO: Fetch current comic page data and push it into WebComicVModel
        WebComicVModel wcvm = new WebComicVModel((BlogHomeVModel)result.Model);
        wcvm.BookName = comic;
        wcvm.ChapterNumber = chapter;
        wcvm.PageNumber = page;
        return View("Index", wcvm);
	}

    
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
	/// Serves the most recent comic page and blog post.
	/// </summary>
	/// <returns></returns>
	public ActionResult MostRecentAll(string comic)
	{
        //TODO: Check that comic is in SeqBook collection. If not then reject.
		//TODO: Consult database for the lastest entries.
		int lastChapter = 1;
		int lastPage = 1;
		int articleId = 2;
		return Comic(comic, lastChapter, lastPage, articleId);
	}

	//TODO:Override PostPage method to alter the values for BlogHomeVModel's controller and action values
}
} //end namespace
