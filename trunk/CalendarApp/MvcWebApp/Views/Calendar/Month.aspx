<%@ Page Language="C#" MasterPageFile="~/Views/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content runat="server" ID="C" ContentPlaceHolderID="Content">

	<%= Jbst.Bind("Calendar.MonthGrid", ViewData["ViewData"])%>

</asp:Content>
