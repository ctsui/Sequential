<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<BlogPostVModel>>" %>
<%@ Import Namespace="Sequential2013.Domain.Models" %>
<% if ( Model.Count()>0){ %>
   <h3>Bulletin Board</h3>
   <div id="StickyNews">
      <% foreach (BlogPostVModel post in Model) { %>   
         <div id="post-<%= Html.Encode(post.BlogPostId) %>">
            <h5><%: Html.ActionLink(post.Title,"GetSinglePost","post",
                                new { id = Html.Encode(post.BlogPostId),
                                title = Html.Encode(post.Title) }, null)%></h5>
            <p><%: post.Excerpt %></p>
            <% if (post.HasExtendedDescription()) { %>
            <%: Html.ActionLink("Read more", "GetSinglePost", "post",
                  new { id = Html.Encode(post.BlogPostId),
                        title=Html.Encode(post.Title) },
                  new { @class="readMoreLink" })%>
            <% } %>
         </div>
         <p class="newsDate"><%: String.Format("{0:MMMM d, yyyy}",post.CreateDate) %></p>
         <hr />
      <% } //end foreach %>
   </div>
<% } //end if (Model.Count()>0) %>