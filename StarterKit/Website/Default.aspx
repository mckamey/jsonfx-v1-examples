<%@ Page Language="C#" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%= System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName %>">
<head runat="server">
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />

    <title>Starter Kit</title>

	<%-- one tag to include all the stylesheets --%>
	<JsonFx:ResourceInclude ID="StyleImport" runat="server" SourceUrl="~/Styles/Styles.merge" />
</head>
<body>
<div class="BodyFade">
	<%-- one tag to include all the scripts --%>
	<JsonFx:ResourceInclude ID="ScriptInclude" runat="server" SourceUrl="~/Scripts/Scripts.merge" />

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
		<p>The StarterKit <strong>requires Cascading Style Sheet support</strong>. Please enable your browser's client script and refresh the page.</p>
	</div>

	<%-- marker CSS class for behavior binding --%>
	<div class="Content js-Content">
		<h1>Loading&hellip;</h1>
		<p style="text-align:center;">If you are seeing this message, then something probably isn't configured properly.  Here are some things to check:</p>

		<noscript>
			<h2>JavaScript</h2>
			<p class="Warning">The StarterKit <strong>requires JavaScript support</strong>. Please enable your browser's client script and refresh the page.</p>
		</noscript>

		<h2>IIS Setup</h2>
		<div class="Warning">
			<p>When hosting the StarterKit in IIS, you will need to map the following extensions to ASP.NET:</p>
			<ul class="BullettedList">
				<li><code>*.merge</code> (client scripts, templates and stylesheets)</li>
				<li><code>*.jsonrpc</code> (JSON-RPC endpoints)</li>
			</ul>

			<p>And optionally if you want to reference these directly instead of via <code>*.merge</code> files:</p>
			<ul class="BullettedList">
				<li><code>*.css</code> (compacted stylesheets)</li>
				<li><code>*.js</code> (compacted client script)</li>
				<li><code>*.jbst</code> (compiled templates)</li>
			</ul>
		</div>

		<h2>Build Errors</h2>
		<p class="Warning">Build errors can cause certain components to fail.  Check the build output in Visual Studio to see if you missed something.</p>
	</div>

	<JsonFx:HistoryManager runat="server"
		Callback="Example.historyCallback" />
</div>
</body>
</html>
