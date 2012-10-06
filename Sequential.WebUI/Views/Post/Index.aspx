<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
         Inherits="System.Web.Mvc.ViewPage<BlogHomeVModel>" %>
<%@ Import Namespace="Sequential2013.Domain.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Story & Art by Carl Tsui
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div id="Blog">
      <%: Html.Partial("~/Views/Shared/BlogPostControl.ascx", Model.RecentPosts) %>
      <%: Html.Partial("~/Views/Shared/PostNavControl.ascx", Model) %>
   </div>
</asp:Content>
