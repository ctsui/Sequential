<%@ Page Language="C#"  MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Comic Not Found
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1 style="text-align:center;padding-top:100px;">
   The comic you were looking for could not be found.
</h1>
<p style="text-align:center;">
This could be because the comic is no longer being hosted or the site set up
has not been completed.
</p>
</asp:Content>
