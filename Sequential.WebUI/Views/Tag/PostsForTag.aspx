<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
         Inherits="System.Web.Mvc.ViewPage<TagHomeVModel>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Posts for &quot;<%: Model.Tag %>&quot;
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <div id="Blog">
      <div id="SearchTerm">
         <h4>Articles tagged &quot;<%: Model.Tag%>&quot;</h4>
      </div>
      <%: Html.Partial("~/Views/Shared/BlogPostControl.ascx",Model.RecentPosts) %>
      <%: Html.Partial("~/Views/Shared/PostNavControl.ascx", Model) %>
      <div id="SearchTerm">
         <h4>Articles tagged &quot;<%: Model.Tag%>&quot;</h4>
      </div>
   </div>
</asp:Content>
