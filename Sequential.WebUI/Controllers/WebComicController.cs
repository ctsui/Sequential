using System.Web.Mvc;
using Sequential2013.Domain.Abstract;
using Sequential2013.Domain.Models;
using System.Web;
using System;
using Sequential2013.Domain;
using Sequential2013.Domain.Concrete.Exception;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

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
   private ISeqBooksRepository booksRep;

   public WebComicController(ISeqPostsRepository pRep,
                              ISeqCategoriesRepository cRep,
                              ISeqTagsRepository tRep,
                              ISeqBooksRepository bRep)
      : base(pRep, cRep, tRep)
   {
      booksRep = bRep;
   }

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
   public ActionResult FullPathEntry(string comic, int chapter, int page,
                                       string perma = null, int id = -1,
                                       bool rootEntry = false)
   {
      //TODO: Check for cookie and if none then set the page number.
      HttpCookie cookie = Request.Cookies.Get(comic + "_bookmark");
      if (cookie == null)
      {
         TimeSpan expirytime = new TimeSpan(120, 0, 0, 0);
         HttpCookie hc = new HttpCookie(comic + "_bookmark", comic + "|1|1");
         hc.Expires = DateTime.Today.Add(expirytime);
         Response.Cookies.Add(hc);
      }
      else //A cookie was found...account for 2 cases...
      {
         if (rootEntry) //User typed in just the base url.
         {
            string[] parts = cookie.Value.Split('|');
            comic = parts[0];
            chapter = Int32.Parse(parts[1]);
            page = Int32.Parse(parts[2]);
         }
         else //User typed in an explicit url to get here so preserve in cookie
         {
            TimeSpan expirytime = new TimeSpan(120, 0, 0, 0);
            cookie.Expires = DateTime.Today.Add(expirytime);
            cookie.Value = comic + "|" + chapter + "|" + page;
            Response.Cookies.Add(cookie);
         }
      }

      ViewResult result;
      //Specific blog identified 
      if (id > 0)
      {
         result = (ViewResult)base.GetSinglePost(comic, id);
      }
      else
      {
         result = (ViewResult)base.Index(comic);
      }
      try
      {
         SeqBook book = booksRep.BookUriContext(comic);
         WebComicVModel wcvm = new WebComicVModel((BlogHomeVModel)result.Model);
         wcvm.BookName = comic;
         wcvm.ChapterNumber = chapter;
         SeqChapter schapter = booksRep.GetChapter(book, chapter);
         wcvm.PageNumber = (page > schapter.PageCount) ? schapter.PageCount : page;
         wcvm.ChapterPageCount = schapter.PageCount;
         wcvm.Controller = "Post";
         wcvm.Action = "AjaxPostPage"; //Need PartialView action like TurnPage
         return View("Index", wcvm);
      }
      //These should only be thrown due to unfinished configuration.
      catch (BookNotFoundException bnfe)
      {
         return View("BookNotFound", bnfe);
      }
      catch (ChapterNotFoundException cnfe)
      {
         return View("ChapterNotFound", cnfe);
      }
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
      //If cookie then extend expires and update the value
      HttpCookie cookie = Request.Cookies.Get(comic + "_bookmark");
      if (cookie != null)
      {
         TimeSpan expirytime = new TimeSpan(120, 0, 0, 0);
         cookie.Expires = DateTime.Today.Add(expirytime);
         cookie.Value = comic + "|" + chapter + "|" + page;
      }
      Response.Cookies.Add(cookie);
      try
      {
         SeqBook book = booksRep.BookUriContext(comic);
         SeqChapter schapter = booksRep.GetChapter(book, chapter);
         WebComicVModel wcvm = new WebComicVModel();
         wcvm.BookName = comic;
         wcvm.ChapterNumber = chapter;
         wcvm.PageNumber = (page > schapter.PageCount) ? schapter.PageCount : page;
         wcvm.ChapterPageCount = schapter.PageCount;
         return PartialView(wcvm);
      }
      catch (BookNotFoundException bnfe)
      {
         return PartialView("BookNotFound", bnfe);
      }
      catch (ChapterNotFoundException cnfe)
      {
         return View("ChapterNotFound", cnfe);
      }
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
      int lastChapter = 1;
      int lastPage = 1;
      return FullPathEntry(comic, lastChapter, lastPage, null, -1, true);
   }

   /// <summary>
	/// Generate a RSS feed of some of the most recent comic page posts. This is
   /// different from recent accompanying blog posts which should use the 
   /// RssFeed method.
	/// </summary>
   /// <param name="comic">The book UriContext value that is the short hand for
   /// the comic for which to build this feed.</param>
	/// <returns>RSS 2.0 feed of the 10 most recent pages of given book.</returns>
   public virtual ContentResult ComicRssFeed(string comic)
   {
      Uri u = Request.RequestContext.HttpContext.Request.Url;
      string applicationPath = Request.RequestContext
                                       .HttpContext.Request
                                       .ApplicationPath;
      string buildDate = String.Format("{0:dddd, MMMM d, yyyy - h:mm tt}",
                                       DateTime.Now);
      //Fetch the last 10 pages updated order by desc pubDate; then make:
      //title = Chapter {chapter num} - Page {page num}
      //link = "{comic}/chapter{chapter}/page{page}/{perma}/{id}"
      //(e.g. http://sputnikx.com/eden/chapter1/page4
      //description = empty or page.Description
      //Rss elements: http://www.ibm.com/developerworks/xml/library/x-rss20/
      try
      {
         SeqBook book = booksRep.BookUriContext(comic);
         List<SeqPage> recentPages = booksRep.GetRecentPages(comic)
                                             .ToList<SeqPage>();
         string linkUri =  u.Scheme + "://" + u.Host + 
                           ((u.Port!=80 || u.Port!=443) ? ":"+u.Port : "") +
                           applicationPath;
         string encoding = Response.ContentEncoding.WebName;
         string feedName = book.Title;
         XDocument rss = new XDocument(
            new XDeclaration("1.0", encoding, "yes"),
            new XElement("rss", new XAttribute("version", "2.0"),
               new XElement("channel", 
                  new XElement("title", feedName),
                  new XElement("link", linkUri),
                  new XElement("description",book.Description),
                  new XElement("language", "en-us"),
                  new XElement("lastBuildDate",buildDate),
                  new XElement("generator", "Sequential 2013"),
                  from page in recentPages
                  select new XElement("item",
                     new XElement(  "title", 
                                    "Chapter "+page.SeqChapter.ChapterNum+
                                    " - Page "+page.PageNum),
                     new XElement(  "link", 
                                    linkUri + comic+"/chapter" +
                                    page.SeqChapter.ChapterNum+ "/page" + 
                                    page.PageNum),
                     new XElement("description", page.Description),
                     new XElement("pubDate", page.PubDate.ToString())
                  ) //XElement item
               ) //XElement channel
            ) //XElement rss
         ); //XDocument
         return Content(rss.ToString(), "application/rss+xml");
      }
      catch (BookNotFoundException bnfe) 
      {
         return Content(bnfe.Message, "text/plain");
      }
      
   }
} //end class WebComicController
} //end namespace
