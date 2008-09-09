/*global JSON, JsonML, JsonFx, Example */
/*
	Example.js

	Example script for the Starter Kit
*/

/* enable valid CSS to target browsers without reverting to CSS hacks */
JsonFx.UI.setCssUserAgent();

/*
	it is a best practice to not clutter the global namespace
	creating top level objects which contain variables and functions
	allows us to simulate namespaces
*/

/* namespace Example */
if ("undefined" === typeof window.Example) {
	window.Example = {};
}

Example.initBrowserList = function(/*DOM*/ elem) {
	// JsonFx.userAgent is a listing of user agent keys
	var data = JsonFx.userAgent;

	var list = Example.browserList.dataBind(data);
	list = JsonML.parse(list, JsonFx.Bindings.bindOne);
	elem.appendChild(list);
};

JsonFx.Bindings.register(
	"div",
	"js-BrowserType",
	Example.initBrowserList,
	null);
