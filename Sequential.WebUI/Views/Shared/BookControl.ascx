<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<BookVModel>>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>
<% if (Model.Count() > 0) { %>
<h3>Featured Comic</h3>
<div id="Books">
<ul>
<% foreach (BookVModel book in Model) { %>   
      <li><%: Html.ActionLink(book.Title, "SinglePage", "comic",
               new { book = Html.Encode(book.Title) }, null)%></li>
<% } %>
</ul>
<p>Finished works can be found in the comics section.</p>
</div>
<% } %>