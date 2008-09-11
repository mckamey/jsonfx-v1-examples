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
<div class="Content">

	<noscript>
		<p class="Warning">These samples <strong>require JavaScript</strong>. Please enable your browser's client script and refresh the page.</p>
	</noscript>

	<div class="js-BrowserType BrowserType">
		<p>Browser details will be bound here.</p>
	</div>

	<h1>Hello world!</h1>

	<p class="Paragraph">Notice how all of the scripts and stylesheets have been automatically compacted, concatenated and Gzip/Deflated (if your browser supports compression).</p>

	<p class="Paragraph">Note: If hosting this in IIS, you will need to map the following extensions to ASP.NET:</p>
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
