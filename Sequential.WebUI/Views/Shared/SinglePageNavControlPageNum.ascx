<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComicVModel>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>
<% if (Model.AtFirst) { %>
First
<% } else { %>
<%: Html.ActionLink("First", "SinglePage", "comic",
    new {
       category = Html.Encode(Model.Title),
       page = Html.Encode(Model.PageNumber),
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
       page = Html.Encode(Model.PageNumber),
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
       page = Html.Encode(Model.PageNumber),
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
       pageNumber = Html.Encode(Model.PageNumber),
       command = "last"
    }, null)%>
<% } %>