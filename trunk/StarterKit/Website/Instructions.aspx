<%@ Page Language="C#" CodeFile="Instructions.aspx.cs" Inherits="Instructions" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%= System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName %>">
<head runat="server">
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />

    <title>JsonFx Setup Instructions</title>

	<%-- one tag to include all the style sheets --%>
	<JsonFx:ResourceInclude ID="StyleImport" runat="server"
		SourceUrl="~/Styles/Styles.merge" />

</head>
<body>
<div class="BodyFade">

	<%-- one tag to include all the scripts --%>
	<JsonFx:ResourceInclude ID="ScriptInclude" runat="server"
		SourceUrl="~/Scripts/Scripts.merge" />

	<div style="display:none;">
		<p>This site <strong>requires Cascading Style Sheet support</strong>. Please enable your browser's client script and refresh the page.</p>
	</div>

	<div class="Content">
		<noscript>
			<p class="Warning">This site <strong>requires JavaScript support</strong>. Please enable your browser's client script and refresh the page.</p>
		</noscript>

		<p><a href="./">&laquo; Return to the JsonFx StarterKit examples</a></p>

		<h1>JsonFx Setup Instructions</h1>

		<h2>IIS Setup</h2>

		<p>When hosting a JsonFx website in IIS 6.0, you will need to map the following extensions to ASP.NET:</p>
		<ul class="BullettedList">
			<li><code><strong>*.merge</strong></code> (client scripts, templates and stylesheets)</li>
			<li><code><strong>*.i18n</strong></code> (localization strings) <strong>See configuration note below.</strong></li>
			<li><code><strong>*.jsonrpc</strong></code> (JSON-RPC endpoints)</li>
		</ul>

		<p>And <em>optionally</em> if you want to reference these directly outside of via <code>*.merge</code> files:</p>
		<ul class="BullettedList">
			<li><code><strong>*.css</strong></code> (compacted style sheets)</li>
			<li><code><strong>*.js</strong></code> (compacted client script)</li>
			<li><code><strong>*.jbst</strong></code> (compiled client-side templates)</li>
		</ul>

		<h3>To setup the extension mappings:</h3>
		<ol class="NumberedList">
			<li>Open IIS Manager ("Start" &raquo; "Run" &raquo; <code>inetmgr</code>)</li>
			<li>Edit the properties of your JsonFx website (right-click &raquo; "Properties")</li>
			<li>Open the Application Configuration Dialog ("Home Directory" tab &raquo; "Configuration&hellip;" button)<br />
				<img src="Styles/IIS_AppConfig.png" alt="IIS 6.0 Application Configuration Dialog" style="padding:1em 0" />
			</li>
			<li>For each mapping to add, click the "Add&hellip;" button and fill out the dialog with the path to the ASP.NET asapi DLL (you can copy this from any of the other mappings)<br />
				<strong>NOTE:</strong> for the <code>*.i18n</code> mapping, <strong>uncheck</strong> the "Verify that file exists" checkbox.
				<img src="Styles/IIS_Mapping.png" alt="IIS 6.0 Application Extension Mapping Dialog" style="padding:1em 0" /><br />
			</li>
		</ol>

		<h2>Visual Studio Editor Setup</h2>

		<p>When developing JBST templates in Visual Studio, you can improve the experience greatly by mapping the <code>*.jbst</code> extension to the UserControl editor.  The syntax is nearly identical so syntax coloring works with JBST templates as well.</p>
		<h3>To setup syntax coloring:</h3>
		<ol class="NumberedList">
			<li>In Visual Studio, close any open JBST templates.</li>
			<li>Open the Options Dialog ("Tools" &raquo; "Options&hellip;")</li>
			<li>Navigate to where you can map extensions to editors ("Text Editor" &raquo; "File Extension")</li>
			<li>Add the extension "<code>jbst</code>" and set the editor to "User Control Editor" and hit "Apply":<br />
				<img src="Styles/VS_Options.png" alt="Visual Studio Options Dialog" style="padding:1em 0" />
			</li>
			<li>Now you should see syntax coloring for all files with the <code>*.jbst</code> extension.</li>
		</ol>

		<h2>Build Errors</h2>
		<p>JsonFx performs <a href="http://jslint.com/" class="js-ExtLink ExtLink">JSLint</a> on client scripts and syntax validation on CSS during compilation. This helps find errors earlier on in the development lifecycle as they show up as build errors directly in Visual Studio's Error List rather than runtime errors.</p>
		<p>Build errors can cause certain components to fail to load.  Check the build output in Visual Studio to see if you missed something.</p>
	</div>

</div>
</body>
</html>
