<%@ Page Language="C#" MasterPageFile="~/Layouts/Layout.Master" Title="DemoApp - TreeView" %>

<script runat="server">

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		this.TV.DataBind();
	}

</script>

<asp:Content runat="server" ContentPlaceHolderID="B">

	<h2>TreeView</h2>

	<jbst:Control runat="server" ID="TV"
		Name="UIT.TreeView"
		InlineData='<%# new DemoApp.BrowseService().Browse("/") %>' />

</asp:Content>
