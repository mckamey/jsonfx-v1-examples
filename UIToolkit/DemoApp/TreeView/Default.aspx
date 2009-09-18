<%@ Page Language="C#" MasterPageFile="~/Layouts/Layout.Master" Title="DemoApp - TreeView" %>

<script runat="server">

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		this.TV1.DataBind();
		this.TV2.DataBind();
	}

</script>

<asp:Content runat="server" ContentPlaceHolderID="B">

	<jbst:Control runat="server" ID="P" Name="Perf" />

	<h2>TreeView</h2>

	<div style="float:left;">
		<jbst:Control runat="server" ID="TV1"
			Name="UIT.FileFolderTree"
			InlineData='<%# new DemoApp.BrowseService().Browse("/") %>' />
	</div>

	<div style="float:left;">
		<jbst:Control runat="server" ID="TV2"
			Name="UIT.ExpandoTree"
			InlineData='<%# new DemoApp.BrowseService().Browse("/") %>' />
	</div>

</asp:Content>
