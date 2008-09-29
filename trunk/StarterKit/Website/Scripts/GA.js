/*global Example, _gat */

/* namespace Example */
if ("undefined" === typeof window.Example) {
	window.Example = {};
}

/*void*/ Example.init = function(/*string*/ id) {
	if (!window.location.port || window.location.port === 80) {
		Example.gat = _gat._getTracker(id);
		Example.gat._trackPageview();
	}
};

/*void*/ Example.track = function(/*string*/ url) {
	if (url && "undefined" !== typeof Example.gat) {
		var domain = window.location.protocol+"//"+window.location.host;
		url = url.replace(domain, "");
		Example.gat._trackPageview(url);
	}
};
