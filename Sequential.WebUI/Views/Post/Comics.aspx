<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Page.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
Comics by Carl Tsui
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="ComicsGallery">
   <table width="640">
      <tr>
         <td>
            <img src="/Content/comics/DreadnoughtSociety.jpg" width="100" height="160" align="left" />
            <h4>"The Dreadnought Society" (Working title)</h4> Victorian / 
            Steampunk adventure in development. An old partner-in-crime
            returns to persuade a discontented society girl to steal the secret
            of British invincibility at sea. 20-22 pages planned.
         </td>
         <td>
            <img src="/Content/comics/LeftOvers1.jpg" width="100" height="160" align="left" />
            <%: Html.ActionLink("The $138 B.L.T.", "SinglePage", "comic", 
                new { book="138BLT", order="asc"}, null) %> - Completed (4 pages).
            How much would you be willing to pay for a B.L.T. sandwich? The answer isn't just what you get to eat.
            Originally published in The Left-Overs of the Living Dead #1 (Fatcat Funnies 2010).
         </td>
      </tr>
      <tr>
         <td>
            <img src="/Content/comics/Inbound4.jpg" width="102" height="160" align="left"/>
            <%: Html.ActionLink("A Method for Reaching Extreme Altitudes", "SinglePage", "comic", 
                new { book="ExtremeAltitudes", order="asc"}, null) %> - Completed (4 pages).
            Documenting Dr. Robert Goddard's early attempts at trying to perfect a rocket engine.
            Originally published in Inbound #4 (Boston Comics Roundtable 2009).
         </td>
         <td>
            <img src="/Content/comics/Inbound3.jpg" width="103" height="160" align="left" />
            <%: Html.ActionLink("The Prince", "SinglePage", "comic", 
                new { book="ThePrince", order="asc"}, null) %> - Completed (4 pages).
            A spiritualist finds love difficult to manipulate even though she has the means to do just that. The
            main character re-appears in my upcoming full-length comic.
            Originally published in Inbound #3 (Boston Comics Roundtable 2009).
         </td>
      </tr>
   </table> 
</div>
</asp:Content>
