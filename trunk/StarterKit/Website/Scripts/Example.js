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
	{ name: "Intro", jbst: Example.introSlide },
	{ name: "CssUserAgent", jbst: Example.userAgentSlide },
	{ name: "Merging", jbst: Example.mergeSlide },
	{ name: "JBST", jbst: Example.jbstSlide },
	{ name: "Services", jbst: Example.servicesSlide }
];

Example.loadSlide = function(/*DOM*/ elem, /*int*/ slide) {
	var template = Example.slides[slide];
	if (!template || !template.jbst) {
		return;
	}
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

Example.initContent = function(/*DOM*/ elem) {
	Example.loadSlide(elem, 0);
};

JsonFx.Bindings.register(
	"div",
	"js-Content",
	Example.initContent,
	null);
