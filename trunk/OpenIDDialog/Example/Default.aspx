<%@ Page Language="C#" %>
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
		OpenID.SignIn.spritePath = '<%= this.ResolveUrl("~/Images/OpenID-Sprite.png") %>';

		function showSignIn() {	
			$("#SignInBtn").hide();
			OpenID.SignIn.show(
				function(/*string*/ url) {
					$("#SignInBtn").val("Your OpenID is:\n"+url).show();
				});
		}

	</script>

	<input id="SignInBtn" type="button" onclick="showSignIn()" value="Sign In" />

</body>
</html>
