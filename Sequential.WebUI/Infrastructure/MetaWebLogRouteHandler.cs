using System.Web;
using System.Web.Routing;
using Sequential2013.Domain.Concrete;
using System.Configuration;

//From Charles Cook's xmlrpc blog
// http://www.cookcomputing.com/blog/archives/xml-rpc-and-asp-net-mvc

public class MetaWeblogRouteHandler : IRouteHandler
{
   public IHttpHandler GetHttpHandler(RequestContext requestContext)
   {

      return new MetaWeblog(ConfigurationManager
                            .ConnectionStrings["AppDb"]
                            .ConnectionString);
   }
}