﻿<%@ Page Language="C#" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%= System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName %>">
<head runat="server">
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />

	<link rel="icon" href="/favicon.ico" type="image/vnd.microsoft.icon" />
	<link rel="shortcut icon" href="/favicon.ico" type="image/vnd.microsoft.icon" />

    <title>Starter Kit</title>

	<%-- one tag to include all the style sheets --%>
	<JsonFx:ResourceInclude ID="StyleImport" runat="server"
		SourceUrl="~/Styles/Styles.merge" />

</head>
<body>
<div class="BodyFade">

	<%-- one tag to include all the scripts --%>
	<JsonFx:ResourceInclude ID="ScriptInclude" runat="server"
		SourceUrl="~/Scripts/Scripts.merge" />

	<%--
		Service proxies are generated at build time
		if application is being run as a virtual directory
		then we need to let the JSON-RPC marshalling system
		know it needs to adjust the end-point URLs
		NOTE: this isn't needed when app root is "/"
	--%>
	<% if (HttpRuntime.AppDomainAppVirtualPath.Length > 1) { %>
		<script type="text/javascript">JsonFx.IO.Service.setAppRoot("<%= HttpRuntime.AppDomainAppVirtualPath %>");</script>
	<% } %>

	<div style="display:none;">
		<p>This site <strong>requires Cascading Style Sheet support</strong>. Please enable your browser's client script and refresh the page.</p>
	</div>

	<%-- marker CSS class for behavior binding --%>
	<div class="Content js-Content">
		<noscript>
			<p class="Warning">This site <strong>requires JavaScript support</strong>. Please enable your browser's client script and refresh the page.</p>
		</noscript>

		<h1 style="margin-top:10em;">Loading&hellip;</h1>

		<p style="text-align:center;">If you can read this message, then something probably isn't configured properly.  Please read the <a href="Instructions.aspx">Setup Instructions</a> for more info.</p>
	</div>

	<JsonFx:HistoryManager runat="server"
		Callback="Example.historyCallback" />
</div>
</body>
</html>
