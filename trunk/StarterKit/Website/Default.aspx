<%@ Page Language="C#" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

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

	<div>
		<p style="display:none;">The StarterKit <strong>requires Cascading Style Sheet support</strong>. Please enable your browser's client script and refresh the page.</p>
	</div>

	<%-- marker CSS class for behavior binding --%>
	<div id="Content" class="Content js-Content">
		<h1>Something isn't working&hellip;</h1>
		<p>If you are seeing this message, then something isn't configured properly.</p>

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

			<p>And optionally these if you want to include them outside of <code>*.merge</code> files:</p>
			<ul class="BullettedList">
				<li><code>*.css</code> (compacted stylesheets)</li>
				<li><code>*.js</code> (compacted client script)</li>
				<li><code>*.jbst</code> (compiled templates)</li>
			</ul>
		</div>
	</div>
</div>
</body>
</html>
