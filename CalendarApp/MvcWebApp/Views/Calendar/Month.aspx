<%@ Page Language="C#" MasterPageFile="~/Views/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content runat="server" ID="C" ContentPlaceHolderID="Content">

	<h1>Month View</h1>

    <h2><%= ViewData["DisplayDate"] %></h2>
    <p>(<%= ViewData["StartRange"] %> to <%= ViewData["EndRange"] %>)</p>

	<%= Jbst.Bind("Calendar.EventList", ViewData["ListData"]) %>

</asp:Content>
