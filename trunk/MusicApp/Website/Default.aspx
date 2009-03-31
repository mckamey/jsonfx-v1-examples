<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MusicApp._Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<%= System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName %>">
<head runat="server">
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=UTF-8" />

	<title>Examples</title>

	<%-- one tag to include all the style sheets --%>
	<JsonFx:ResourceInclude runat="server" SourceUrl="~/Styles/Styles.merge" />
</head>
<body>

	<%-- one tag to include all the client scripts --%>
	<JsonFx:ResourceInclude runat="server" SourceUrl="~/Scripts/Scripts.merge" />

	<%--
		Service proxies are generated at build time, so
		if application is being run as a virtual directory
		then we need to let the JSON-RPC marshalling system
		know it needs to adjust the service end-point URLs
		NOTE: you can remove this if app root will always be "/"
	--%>
	<% if (HttpRuntime.AppDomainAppVirtualPath.Length > 1) { %>
		<script type="text/javascript">JsonFx.IO.Service.setAppRoot("<%= HttpRuntime.AppDomainAppVirtualPath %>");</script>
	<% } %>

<div class="content">
	<div>
		<a class="button" href="#modal" onclick="return Music.Dialog.show(Music.Lipsum,1,320,240,true);">Modal Dialog</a>
		<a class="button" href="#modeless" onclick="return Music.Dialog.show(Music.Lipsum,5,-1,-1,false);">Modeless Dialog</a>
		<a class="button" href="#toggle-loading" onclick="return $(this).toggleClass('button-active').is('.button-active') ? Music.Loading.show() : Music.Loading.hide();">Loading&hellip;</a>
	</div>

	<jbst:control id="Menu" runat="server" name="Music.Menu" />
</div>

<div>
<h2>TODO:</h2>
<ul>
	<li>explain how to switch templates in list JBSTs</li>
	<li>explain using Linq / Anonymous objects with JSON-RPC</li>
	<li>explain inserting markup data into JBST</li>
	<li>explain zebra striping</li>
	<li>explain difference between JsonFx.UI.Binding vs. jQuery.ready</li>
	<li>explain JSON-RPC callback examples</li>
</ul>
</div>

</body>
</html>
