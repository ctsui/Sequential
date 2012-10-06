<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
         Inherits="System.Web.Mvc.ViewPage<PostsForCategoryVModel>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Posts for &quot;<%: Model.Category %>&quot;
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div id="Blog">   
      <div id="SearchTerm">
         <h4>Articles categorized &quot;<%: Model.Category%>&quot;</h4>
      </div>
      <%: Html.Partial("~/Views/Shared/BlogPostControl.ascx",Model.AllPosts) %>
      <%: Html.Partial("~/Views/Shared/PostNavControl.ascx", Model) %>
       <div id="SearchTerm">
         <h4>Articles categorized &quot;<%: Model.Category%>&quot;</h4>
      </div>
   </div>
</asp:Content>
