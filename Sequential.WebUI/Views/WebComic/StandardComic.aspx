<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Anthology.Master" Inherits="System.Web.Mvc.ViewPage<ComicVModel>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%: Model.Title %> - by Carl Tsui
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="SideTOC">
   <h1><%: Model.Title %></h1>
   <p><%: Html.ActionLink("Continuous View","VerticalPages","comic",
                     new { book=Html.Encode(Model.UriContext)},null) %></p>
   <br />
   <p><%: Html.ActionLink("Comics List", "ComicsGallery", "Post")%></p>
   <p><%: Html.ActionLink("Go to Blog","Index","Post") %></p>

   <br />
   <% if (Model.CurrentPage.ExtendedDescription != "") { %>
   <h3>Page Note</h3>
   <p><%= Model.CurrentPage.ExtendedDescription%></p>
   <% } %>
</div>
<div id="Post">
   <%: Html.Partial("~/Views/Shared/SinglePageNavControl.ascx")%>
   <%= Model.CurrentPage.Description %>
   <%: Html.Partial("~/Views/Shared/SinglePageNavControl.ascx")%>
   <br />
   <p class="powered">Powered by Sequential</p>
</div>
</asp:Content>
