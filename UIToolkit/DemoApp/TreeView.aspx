<%@ Page Language="C#" MasterPageFile="~/Layouts/Layout.Master" %>

<script runat="server">

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		this.TV.DataBind();
	}

</script>

<asp:Content runat="server" ContentPlaceHolderID="B" ID="Body">

	<jbst:Control runat="server" ID="TV"
		Name="JbstUIToolkit.TreeView.TreeRoot"
		InlineData='<%# new DemoApp.BrowseService().Browse("/") %>' />

</asp:Content>