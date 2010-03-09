/*global _gat */

/* namespace Example */
var Example;
if ("undefined" === typeof window.Example) {
	window.Example = {};
}

/*void*/ Example.initGA = function(/*string*/ id) {
	if ("undefined" !== typeof _gat &&
		!document.location.port || document.location.port === 80 &&
		document.location.hostname.toLowerCase().indexOf("jsonfx.net") >= 0) {
		Example.gat = _gat._getTracker(id);
		Example.gat._trackPageview();
	}
};

/*void*/ Example.track = function(/*string*/ url) {
	if (url && "undefined" !== typeof Example.gat) {
		var domain = location.protocol+"//"+location.host;
		url = url.replace(domain, "");
		Example.gat._trackPageview(url);
	}
};
