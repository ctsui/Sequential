using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web;

namespace Sequential2013.WebUI.Infrastructure
{
public static class ImageActionLinkHelper
{
	public static IHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes=null)
	{
		var builder = new TagBuilder("img");
		builder.MergeAttribute("src", imageUrl);
		builder.MergeAttribute("alt", altText);
		//builder.MergeAttribute("border", "0");
		var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions, htmlAttributes).ToHtmlString();
		return new MvcHtmlString(link.Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
	} 

} //end class
} //end namespace