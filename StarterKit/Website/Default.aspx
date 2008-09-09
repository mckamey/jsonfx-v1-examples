<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%= System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName %>">
<head runat="server">
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />

    <title>Starter Kit</title>

	<%-- one tag to include all the stylesheets --%>
	<JsonFx:ResourceInclude ID="StyleImport" runat="server" SourceUrl="/Styles/Styles.merge" />
</head>
<body>

<%-- one tag to include all the scripts --%>
<JsonFx:ResourceInclude ID="ScriptInclude" runat="server" SourceUrl="/Scripts/Scripts.merge" />

<div>
<form id="F" runat="server">
	<h1>Hello world!</h1>
	<p>Notice how all of the scripts and stylesheets have been automatically compacted, concatenated and Gzip/Deflated (if your browser supports compression).</p>
</form>
</div>

</body>
</html>
