<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Anthology.Master"
         Inherits="System.Web.Mvc.ViewPage<ComicVModel>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: Model.Title %> - A Comic by Carl Tsui
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <a name="top"><div id="Spacer"></div></a>
   <div id="SideTOC">
      <h1><%: Model.Title %></h1>
      <p><%: Html.ActionLink("First Page Top","VerticalPages","comic",
             new { category=Html.Encode(Model.Title), order="asc"}, null) %></p>
      <p><%: Html.ActionLink("Last Page Top","VerticalPages","comic",
             new { category=Html.Encode(Model.Title), order="desc"}, null) %></p>
      <br />
      <p><%: Html.ActionLink("Single Page View","SinglePage","comic",
             new { category=Html.Encode(Model.Title), order="desc"}, null) %></p>
      <br />  
      <p><%: Html.ActionLink("Comics List", "ComicsGallery", "Post")%></p>
      <p><%: Html.ActionLink("Go to Blog","Index","Post") %></p>
   </div>
   <div id="Post">
   <% foreach (BlogPostVModel post in Model.VerticalPages) { %>   
      <%= post.Description %>
   <% } %>
   <p style="text-align:center"><a href="#top">Top of page</a></p>
   <br />
   <p class="powered">Powered by Sequential</p>
   </div>
</asp:Content>
