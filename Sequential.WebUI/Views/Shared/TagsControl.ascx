<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BlogPostVModel>" %>
<%@ Import Namespace="Sequential2013.Domain.Models" %>
<% if (Model.HasTags()) { %>
   <p>
   Tagged:
   <% 
      int totalTags = Model.Tags.Count();
      int tagCount = 0;
      foreach (TagVModel t in Model.Tags) { 
   %>
   <%: Html.ActionLink(Html.Encode(t.Name), "PostsForTag", "tag",
         new { tagName=t.Name,tagId = t.TagId, pageNum=1}, null)%>
      <% tagCount++;
         if (tagCount < totalTags) { %>
   ,
      <% } %>
   <% } %>
   </p>
<% } %>
