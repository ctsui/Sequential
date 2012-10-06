<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<CategoryVModel>>" %>
<%@ Import Namespace="Sequential2013.Domain.Models" %>

<div id="Categories">
<h3>Topics</h3>
<% if (Model.Count() > 0) { %>
   <ul>
   <% foreach (CategoryVModel cat in Model) { %>
         <li><%: Html.ActionLink(cat.ToString(), "PostsForCategory", "category",
                  new { category = Html.Encode(cat.Name), pageNum = 1 }, null)%></li>
   <% } %>
   </ul>
<% } else { %>
<p style="padding-left:40px;">None yet. Write more!</p>
<% } %>
</div>
