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
	// clear the container contents
	JsonFx.DOM.clear(elem);

	// JsonFx.userAgent is a listing of user agent keys
	var data = JsonFx.userAgent;

	// this databinds the data to the template
	var list = Example.browserList.dataBind(data);

	// this hydrates the resulting markup allowing dynamic behaviors to be bound to elements
	list = JsonML.parse(list, JsonFx.Bindings.bindOne);

	// add the result to the container
	elem.appendChild(list);
};

JsonFx.Bindings.register(
	"div",
	"js-BrowserType",
	Example.initBrowserList,
	null);

// slides are just a list of JBST templates
Example.slides = [
	Example.introSlide,
	Example.servicesSlide
];

Example.loadSlide = function(/*DOM*/ elem, /*int*/ slide, /*object*/ data) {
	// search up ancestors to find container with marker className
	elem = JsonFx.DOM.findParent(elem, "js-Content");

	// clear the container contents
	JsonFx.DOM.clear(elem);

	var template = Example.slides[slide];
	if (!template) {
		throw new Error("Slide #"+slide+" cannot be found.");
	}

	// this databinds the data to the template
	var list = template.dataBind(data||"");

	// this hydrates the resulting markup allowing dynamic behaviors to be bound to elements
	list = JsonML.parse(list, JsonFx.Bindings.bindOne);

	// add the result to the container
	elem.appendChild(list);
};

Example.initContent = function(/*DOM*/ elem) {
	Example.loadSlide(elem, 0);
};

JsonFx.Bindings.register(
	"div",
	"js-Content",
	Example.initContent,
	null);
