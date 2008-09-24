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
	{ name: "Intro", title: "Hello World!", jbst: Example.introSlide },
	{ name: "JBST", title: "JsonML + Browser-Side Templating", jbst: Example.jbstSlide },
	{ name: "Services", title: "Ajax Services", jbst: Example.servicesSlide },
	{ name: "Performance", title: "Performance Optimizations", jbst: Example.mergeSlide },
	{ name: "CssUserAgent", title: "UserAgent-Specific CSS", jbst: Example.userAgentSlide },
	{ name: "Behaviors", title: "JavaScript Behavior Bindings", jbst: Example.behaviorSlide },
	{ name: "i18n/L10n", title: "Client-Side Globalization", jbst: Example.globalizationSlide },
	{ name: "History", title: "Ajax History Support", jbst: Example.historySlide },
	{ name: "Download", title: "StarterKit Download", jbst: Example.downloadSlide }
];

Example.curSlide = NaN;

Example.loadSlide = function(/*int*/ slide) {
	if (Example.curSlide === slide || !Example.slides[slide] || !Example.slides[slide].jbst) {
		return;
	}

	// this triggers JsonFx.History.onchange and
	// the callback calls Example.loadSlideInternal
	// this way when a history event is triggered
	// it uses the same code as user actions
	JsonFx.History.save( { slide: slide } );
};

Example.loadSlideInternal = function(/*int*/ slide) {
	if (Example.curSlide === slide) {
		return;
	}

	var template = Example.slides[slide];
	if (!template || !template.jbst) {
		return;
	}

	Example.curSlide = slide;
	
	template = template.jbst;

	// find container with marker className
	var elem = Example.container || JsonFx.DOM.findChild(document.body, "js-Content");

	// clear the container contents
	JsonFx.DOM.clear(elem);

	// this databinds the data to the template
	var list = template.dataBind( { "slide" : slide, "count" : Example.slides.length } );

	// this hydrates the resulting markup allowing dynamic behaviors to be bound to elements
	list = JsonML.parse(list, JsonFx.Bindings.bindOne);

	// add the result to the container
	if (elem && list) {
		elem.appendChild(list);
	}
};

/*
	initialize behavior binding
*/
JsonFx.Bindings.register(
	"div",
	"js-Content",
	function(/*DOM*/ elem) {
		Example.container = elem;
		Example.loadSlide(0);
	},
	null);

/* allow the user to navigate with arrow keys */
/*void*/ document.onkeydown = function(/*Event*/ evt) {
	evt = evt||window.event;

	switch (JsonFx.DOM.getKeyCode(evt)) {
		case 0x25: // left arrow
			Example.loadSlide( (Example.curSlide+Example.slides.length-1)%Example.slides.length );
			break;

		case 0x26: // up arrow
			Example.loadSlide( 0 );
			break;

		case 0x27: // right arrow
			Example.loadSlide( (Example.curSlide+1)%Example.slides.length );
			break;

		case 0x28: // down arrow
			Example.loadSlide( Example.slides.length-1 );
			break;

		default:
//			alert(JsonFx.DOM.getKeyCode(evt));
			return;
	}
	JsonFx.DOM.clearEvent(evt);
};

/* Ajax history callback */
Example.historyCallback = function(/*object*/ info) {
	if (!info) {
		return;
	}

	if ("undefined" !== typeof info.slide) {
		Example.loadSlideInternal( info.slide );
	}
};