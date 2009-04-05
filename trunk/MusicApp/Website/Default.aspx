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
	<jbst:control id="Menu" runat="server" name="Music.Menu" />
	<jbst:control id="Grid" runat="server" name="Music.ArtistGrid" />
	<jbst:control id="List" runat="server" name="Music.ArtistList" />
</div>

<div>
<h2>TODO:</h2>
<ul>
	<li>alter the sort closure data when adding</li>
	<li>hook up swapping views between grid/list</li>
	<li>add delete to edit actions with "Are you sure?" dialog and fix zebra striping</li>
	<li>Replace Wikipedia iframe with rich markup</li>
	<li>explain how to switch templates in list JBSTs</li>
	<li>explain inserting markup data into JBST</li>
	<li>explain difference between JsonFx.UI.Binding vs. jQuery.ready</li>
	<li>explain JSON-RPC callback examples</li>
</ul><ul>
	<li>demonstrate sending strongly typed objects via JSON-RPC</li>
	<li>demonstrate inline edit templates</li>
	<li>demonstrate <a href="http://www.alistapart.com/articles/zebrastripingmoredataforthecase/" class="ext-link">zebra striping</a></li>
	<li>demonstrate using Linq / Anonymous objects within JSON-RPC</li>
</ul>
</div>

</body>
</html>
