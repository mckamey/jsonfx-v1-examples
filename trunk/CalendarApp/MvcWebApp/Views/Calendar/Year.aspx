﻿<%@ Page Language="C#" MasterPageFile="~/Views/Layout.master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content runat="server" ID="C" ContentPlaceHolderID="Content">

	<h1><%= ViewData["DisplayDate"] %> [ Year View ]</h1>

	<%= Jbst.Bind("Calendar.EventList", ViewData["ViewData"])%>

</asp:Content>
