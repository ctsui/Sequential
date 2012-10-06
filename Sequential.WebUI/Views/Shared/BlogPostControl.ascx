<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<BlogPostVModel>>" %>
<%@ Import Namespace="Sequential2013.Domain.Models" %>
<%@ Import Namespace="System" %>
 <% if (Model.Count() == 0) { %>
   <p>No more posts.</p>
   <% } %>
   <% foreach (BlogPostVModel post in Model) { %>   
   <div id="post-<%= Html.Encode(post.BlogPostId) %>">
      <h2>
         <%: Html.ActionLink(post.Title,"GetSinglePost","post",
                              new { id = Html.Encode(post.BlogPostId),
                                    title = Html.Encode(post.Permalink) }, null)%>
      </h2>
      <%= post.Description %>
      <% if (post.HasExtendedDescription()) { %>
         <p class="readMoreLink">
         <%: Html.ActionLink("> read more", "GetSinglePost", "post",
               new { id = Html.Encode(post.BlogPostId),
                     title=Html.Encode(post.Permalink) },
               new { @class="readMoreLink" })%>
         </p>
      <% } %>
   </div>
   <%: Html.Partial("~/Views/Shared/TagsControl.ascx",post) %>
   <% 
      TimeZoneInfo estZ = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); 
      DateTime estTime = TimeZoneInfo.ConvertTime(post.CreateDate, TimeZoneInfo.Utc, estZ);
    %>
   <p class="datetime">
      <%: Html.Encode(String.Format("{0:dddd, MMMM d, yyyy - h:mm tt} ({1})", 
                      estTime, 
                      estZ.IsDaylightSavingTime(estTime)? estZ.DaylightName : estZ.StandardName)) %>
   </p>
   <hr />
<% } %> <!-- end foreach -->