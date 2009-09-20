<%@ Page Language="C#" MasterPageFile="~/Layouts/Layout.Master" Title="DemoApp - TreeView" %>

<asp:Content runat="server" ContentPlaceHolderID="B">

	<h2>TreeView</h2>

	<div style="float:left;">
		<jbst:Control runat="server"
			Name="UIT.FileFolderTree"
			InlineData='<%# new DemoApp.BrowseService().Browse("/") %>' />
	</div>

	<div style="float:left;">
		<jbst:Control runat="server"
			Name="UIT.ExpandoTree"
			InlineData='<%# new DemoApp.BrowseService().Browse("/") %>' />
	</div>

</asp:Content>
