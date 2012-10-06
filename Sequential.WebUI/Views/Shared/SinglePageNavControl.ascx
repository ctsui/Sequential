<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComicVModel>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>
<div id="SinglePageNavControl">
<% if (Model.AtFirst) { %>
First
<% } else { %>
<%: Html.ActionLink("First", "SinglePage", "comic",
    new {
       category = Html.Encode(Model.Title),
       postId = Html.Encode(Model.CurrentPage.BlogPostId),
       command = "first"
    }, null)%>
<% } %>
 | 
<% if (Model.AtFirst) { %>
Previous
<% } else { %>
<%: Html.ActionLink("Previous", "SinglePage", "comic",
    new {
       category = Html.Encode(Model.Title),
       postId = Html.Encode(Model.CurrentPage.BlogPostId),
       command = "previous"
    }, null)%>
<% }%>
 | 
<% if (Model.AtLast) { %>
Next
<% } else { %>
<%: Html.ActionLink("Next", "SinglePage", "comic",
    new {
       category = Html.Encode(Model.Title),
       postId = Html.Encode(Model.CurrentPage.BlogPostId),
       command = "next"
    }, null)%>
 
 <% } %>
 | 
 <% if (Model.AtLast) { %> 
 Last
<% } else { %>
 <%: Html.ActionLink("Last", "SinglePage", "comic",
    new {
       category = Html.Encode(Model.Title),
       postId = Html.Encode(Model.CurrentPage.BlogPostId),
       command = "last"
    }, null)%>
<% } %>
</div>