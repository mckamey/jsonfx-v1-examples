﻿<%@ Page Language="C#" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%= System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName %>">
<head runat="server">
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />

	<link rel="icon" href="/favicon.ico" type="image/vnd.microsoft.icon" />
	<link rel="shortcut icon" href="/favicon.ico" type="image/vnd.microsoft.icon" />

    <title>JsonFx StarterKit</title>

	<%-- one tag to include all the style sheets --%>
	<JsonFx:ResourceInclude runat="server" SourceUrl="~/Styles/Styles.merge" />

</head>
<body>
<div class="BodyFade">

	<%-- one tag to include all the scripts --%>
	<JsonFx:ResourceInclude runat="server" SourceUrl="~/Scripts/Scripts.merge" />
	<JsonFx:ScriptDataBlock runat="server" ID="PageData" />

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

	<jbst:Control id="slideFrame" runat="server">
		<%-- default content gets replaced with dynamic content on load --%>
		<div class="Frame">
			<div class="Content">
				<noscript>
					<p class="Warning">This site <strong>requires JavaScript support</strong>. Please enable your browser's client script and refresh the page.</p>
				</noscript>

				<h1 style="padding-top:150px;">Loading&hellip;</h1>

				<p style="text-align:center;">If you can read this message, then something probably isn't configured properly.  Please read the <a href="http://help.jsonfx.net/instructions">Setup Instructions</a> for more info.</p>
			</div>
		</div>
	</jbst:Control>

	<JsonFx:HistoryManager runat="server"
		Callback="Example.historyCallback" />

	<script type="text/javascript" src="http://www.google-analytics.com/ga.js"></script>
	<script type="text/javascript">Example.initGA("UA-1294169-8");</script>
</div>
</body>
</html>
