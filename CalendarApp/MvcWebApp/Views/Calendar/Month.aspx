<%@ Page Language="C#" MasterPageFile="~/Views/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content runat="server" ID="C" ContentPlaceHolderID="Content">

	<h1>Month View</h1>

    <h2><%= ViewData["UserDate"] %></h2>

</asp:Content>
