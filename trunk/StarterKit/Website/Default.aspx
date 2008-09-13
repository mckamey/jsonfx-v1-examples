<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%= System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName %>">
<head runat="server">
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />

    <title>Starter Kit</title>

	<%-- one tag to include all the stylesheets --%>
	<JsonFx:ResourceInclude ID="StyleImport" runat="server" SourceUrl="~/Styles/Styles.merge" />
</head>
<body>

<%-- one tag to include all the scripts --%>
<JsonFx:ResourceInclude ID="ScriptInclude" runat="server" SourceUrl="~/Scripts/Scripts.merge" />

<form id="F" runat="server">

	<p class="CssWarning">These samples <strong>require Cascading Style Sheet support</strong>. Please enable your browser's client script and refresh the page.</p>

	<noscript>
		<p class="Warning">These samples <strong>require JavaScript support</strong>. Please enable your browser's client script and refresh the page.</p>
	</noscript>

	<div class="js-Content">
		<h1>Something isn't working&hellip;</h1>
		<p>If you are seeing this message, then something isn't configured properly.</p>

		<h2>IIS Setup</h2>
		<p>When hosting the StarterKit in IIS, you will need to map the following extensions to ASP.NET:</p>
		<ul class="BulletedList">
			<li>*.css</li>
			<li>*.js</li>
			<li>*.merge</li>
			<li>*.jbst</li>
			<li>*.jsonrpc</li>
		</ul>
	</div>
</form>

</body>
</html>
