<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
         Inherits="System.Web.Mvc.ViewPage<BlogHomeVModel>" %>
<%@ Import Namespace="Sequential2013.Domain.Models" %>
<%@ Import Namespace="System" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.RecentPosts[0].Title %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="Blog">   
   <div id="post">
      <h2><%: Model.RecentPosts[0].Title %></h2>
      <%= Model.RecentPosts[0].FullPostText() %>
   </div>
   <br />
   <%: Html.Partial("~/Views/Shared/TagsControl.ascx",Model.RecentPosts[0]) %>
   <% 
      TimeZoneInfo estZ = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); 
      DateTime estTime = TimeZoneInfo.ConvertTime(Model.RecentPosts[0].CreateDate, TimeZoneInfo.Utc, estZ);
    %>
   <p class="datetime">
      <%: Html.Encode(String.Format("{0:dddd, MMMM d, yyyy - h:mm tt} ({1})", 
                      estTime, 
                      estZ.IsDaylightSavingTime(estTime)? estZ.DaylightName : estZ.StandardName)) %>
   </p>
   <br />
</div>
</asp:Content>
