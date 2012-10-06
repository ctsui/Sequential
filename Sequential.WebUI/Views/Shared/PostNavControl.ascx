<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PagedVModel>" %>
<%@ Import Namespace="Sequential2013.Domain.Models" %>
<div id="postNavControl">
<% if (Model.CurrentPage > 1) { %>
<%: Html.ActionLink("Newer Posts", Html.Encode(Model.Action), Html.Encode(Model.Controller),
      new { pageNum = Html.Encode(Model.CurrentPage - 1)}, null)%>
 | 
<% } %>
<%: Html.ActionLink("Home", "PostPage", "post", new {pageNum=1},null) %>
<% if (Model.HasMorePages) { %>
 | 
<%: Html.ActionLink("Older Posts", Html.Encode(Model.Action), Html.Encode(Model.Controller),
      new { pageNum=Html.Encode(Model.CurrentPage+1)}, null)%>
<% } %>
</div>