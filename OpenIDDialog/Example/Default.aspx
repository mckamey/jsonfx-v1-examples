<%@ Page Language="C#" %>

<script runat="server">

	private DotNetOpenAuth.OpenId.Identifier indentifier;
	private string friendly;
	
	protected override void OnLoad(EventArgs e)
	{
		this.Context.RewritePath("~/", true);
		
		this.indentifier =
			new OpenIDDialog.Services.OpenIDService().EndAuthentication(out this.friendly);
		
		base.OnLoad(e);
	}

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>OpenID Dialog Example</title>

	<%-- one tag to include all the style sheets --%>
	<JsonFx:ResourceInclude runat="server" SourceUrl="~/Styles.merge" />
</head>
<body>

	<%-- one tag to Google-hosted jQuery --%>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.js"></script>

	<%-- one tag to include all the client scripts --%>
	<JsonFx:ResourceInclude runat="server" SourceUrl="~/Scripts.merge" />

	<script type="text/javascript">
		/* you can remove this when app root will always be "/" */
		JsonFx.IO.Service.setAppRoot("<%= HttpRuntime.AppDomainAppVirtualPath %>");
		OpenID.Settings.spritePath = '<%= this.ResolveUrl("~/Images/OpenID-Sprite.png") %>';

		function beginAuth(/*string*/ authid) {
			OpenID.Service.beginAuth(
				authid,
				window.location.href,
				{
					onSuccess: function(/*string*/ url) {
						if (url) {
							window.location.href = url;
						}
					}
				});
		}

		function signOut() {
			alert("TODO: sign-out.");
		}

	</script>

<div style="padding:10px;">
	<% if (String.IsNullOrEmpty(this.friendly)) { %>
		<a href="#signin" onclick="OpenID.SignIn.show(beginAuth);return false;">Sign In</a>
	<% } else { %>
		<h1>Welcome, <%= this.friendly %>!</h1>
		<a href="#signout" onclick="signOut()">Sign Out</a>
	<% } %>
</div>

</body>
</html>
