using System.Web.Mvc;
using System.Web.Routing;
using Sequential2013.WebUI.Infrastructure;

namespace Sequential2013.WebUI
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			/* MetaWeblogRouteHandler entry originally from Charles Cook
				http://www.cookcomputing.com/blog/archives/xml-rpc-and-asp-net-mvc 
				This entry must precede the MVC routing or the dependency
				injector will fail trying to instantiate a controller for
				the route. */
			routes.Add(new Route("{weblog}", null,
				new RouteValueDictionary(new { weblog = "xmlrpc" }),
				new MetaWeblogRouteHandler()));

			routes.MapRoute("PostNav", "posts/page/{pageNum}",
			   new { controller = "Post",
			         action = "PostPage"
			   });

			/* localhost/sequential/post/{id}/{title} - note that the title is
            simply a human recognizable condensation of the actual post title
            but everything will really be looked up by id. */
         routes.MapRoute("Posts", "read/{id}/{title}",
            new { controller="Post",
                  action="GetSinglePost",
                  title=UrlParameter.Optional});

			/* localhost/sequential/category/{category} */
			routes.MapRoute("Categories", "category/{category}/{pageNum}/{order}",
				new {
					controller = "Category",
					action = "PostsForCategory",
					category = "Uncategorized",
					order = "desc"
				});

			routes.MapRoute("Tag", "tagged/{tagName}/{tagId}/{pageNum}/{order}",
				new {
					controller = "Tag",
					action = "PostsForTag",
					tagName = UrlParameter.Optional,
					order = "desc"
				});

			routes.MapRoute("Rss", "rss/",
				new {
					controller = "Post",
					action = "RssFeed"
				});

			routes.MapRoute("WebcomicExplicit", 
                "{comic}/chapter{chapter}/page{page}/{perma}/{id}",
				new { 
					controller = "WebComic", 
					action = "Comic",
					perma=UrlParameter.Optional,
					id=UrlParameter.Optional
				},
				new { 
					comic=@"[\w\d]+", 
					chapter = @"\d+", 
					page = @"\d+",
					perma = @"[\w\d!()_.-]*",
					id = @"\d*" 
				});

            routes.MapRoute("TurnPage", "turnpage/{comic}/chapter{chapter}/page{page}",
                new
                {
                    controller = "WebComic",
                    action = "TurnPage"
                });

            routes.MapRoute("WebcomicHome", "{comic}",
                new
                {
                    controller = "WebComic",
                    action = "MostRecentAll"
                });

            routes.MapRoute(
                 "Default", // Route name
                 "{controller}/{action}/{id}", // URL with parameters
                 new { controller = "Post", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
			
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);
			RegisterGlobalFilters(GlobalFilters.Filters);
			ControllerBuilder.Current.SetControllerFactory(
				new NinjectControllerFactory());
		}
	}
}