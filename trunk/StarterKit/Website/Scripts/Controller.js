/*global JSON, JsonML, JsonFx, Example */
/*
	Example.js

	Example controller for the Starter Kit
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

// slides are just a list of JBST templates
Example.slides = [
	{ name: "Intro", title: "Welcome to JsonFx", jbst: Example.introSlide },
	{ name: "Templates", title: "JsonML + Browser-Side Templating", jbst: Example.jbstSlide },
	{ name: "Services", title: "Ajax Services", jbst: Example.servicesSlide },
	{ name: "Merging", title: "Resource Merging", jbst: Example.mergeSlide },
	{ name: "CssUserAgent", title: "UserAgent-Specific CSS", jbst: Example.userAgentSlide },
	{ name: "Behaviors", title: "JavaScript Behavior Bindings", jbst: Example.behaviorSlide },
	{ name: "i18n/L10n", title: "Client-Side Globalization", jbst: Example.globalizationSlide },
	{ name: "History", title: "Ajax History Support", jbst: Example.historySlide },
	{ name: "Download", title: "JsonFx Starter Kit", jbst: Example.starterKitSlide }
];

Example.curSlide = NaN;

Example.loadSlide = function(/*DOM*/ elem, /*int*/ slide) {
	if (Example.curSlide === slide) {
		return;
	}

	var template = Example.slides[slide];
	if (!template || !template.jbst) {
		return;
	}

	Example.curSlide = slide;
	
	template = template.jbst;

	// search up ancestors to find container with marker className
	elem = JsonFx.DOM.findParent(elem, "js-Content");

	// clear the container contents
	JsonFx.DOM.clear(elem);

	// this databinds the data to the template
	var list = template.dataBind( { "slide" : slide, "count" : Example.slides.length } );

	// this hydrates the resulting markup allowing dynamic behaviors to be bound to elements
	list = JsonML.parse(list, JsonFx.Bindings.bindOne);

	// add the result to the container
	elem.appendChild(list);
};

/*
	initialize behavior binding
*/
JsonFx.Bindings.register(
	"div",
	"js-Content",
	function(/*DOM*/ elem) {
		Example.loadSlide(elem, 0);
	},
	null);
