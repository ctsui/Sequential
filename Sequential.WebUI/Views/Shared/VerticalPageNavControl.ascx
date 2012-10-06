<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComicVModel>" %>
<%@Import Namespace="Sequential2013.Domain.Models" %>
<%: Html.ActionLink("Read From The Beginning","VerticalPages","comic",
    new { category=Html.Encode(Model.Title), order="asc"}, null) %>
 |
<%: Html.ActionLink("Most Recent First","VerticalPages","comic",
    new { category=Html.Encode(Model.Title), order="desc"}, null) %>