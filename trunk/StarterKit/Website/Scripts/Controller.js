/*global JSON, JsonML, JsonFx, TreeNode, Example */
/*
	Example.js

	Example controller for the Starter Kit
*/

/* enable valid CSS to target browsers without resorting to CSS hacks */
JsonFx.UA.setCssUserAgent();

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
	{ name: "JBST", title: "JBST: JsonML+Browser-Side Templating", jbst: Example.jbstSlide },
	{ name: "Services", title: "Ajax Services", jbst: Example.servicesSlide },
	{ name: "Performance", title: "Performance Optimizations", jbst: Example.mergeSlide },
	{ name: "CssUserAgent", title: "UserAgent-Specific CSS", jbst: Example.userAgentSlide },
	{ name: "Behaviors", title: "Dynamic Behavior Binding", jbst: Example.behaviorSlide },
	{ name: "i18n/L10n", title: "Client-Side Globalization", jbst: Example.globalizationSlide },
	{ name: "History", title: "Ajax History Support", jbst: Example.historySlide },
	{ name: "Source Code", title: "Browse The Source Code", jbst: Example.downloadSlide }
];

Example.curSlide = NaN;

Example.loadSlide = function(/*int*/ slide) {
	// normalize slide number
	slide = (isFinite(slide) && slide > 0) ? (Number(slide) % Example.slides.length) : 0;

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
	var template = Example.slides[slide];
	if (!template || !template.jbst) {
		return;
	}

	Example.curSlide = slide;
	
	template = template.jbst;

	// find container with marker className
	var elem = document.getElementById("frame");

	// clear the container contents
	JsonFx.UI.clear(elem);

	// this databinds the template
	// we can use the index and count properties to let the template know which slide is being bound
	var list = template.bind({}, slide, Example.slides.length);

	// add the result to the container
	if (elem && list) {
		elem.appendChild(list);
		Example.track("/slide_"+Example.curSlide);
	}
};

/*
	initialize behavior binding
*/
JsonFx.Bindings.add(
	"#frame",
	function(/*DOM*/ elem) {
		var slide = Number(window.location.hash && window.location.hash.substr(1));
		Example.loadSlide(slide);

		if (document.location.hostname.toLowerCase().indexOf("jsonfx.net") >= 0) {
			Example.initGA("UA-1294169-8");
		}
	});

/* allow the user to navigate with arrow keys */
/*void*/ document.onkeydown = function(/*Event*/ evt) {
	evt = evt||window.event;

	switch (TreeNode.getKeyCode(evt)) {
		case 0x25: // left arrow
			Example.loadSlide( (Example.curSlide+Example.slides.length-1)%Example.slides.length );
			break;

//		case 0x26: // up arrow
//			Example.loadSlide( 0 );
//			break;

		case 0x27: // right arrow
			Example.loadSlide( (Example.curSlide+1)%Example.slides.length );
			break;

//		case 0x28: // down arrow
//			Example.loadSlide( Example.slides.length-1 );
//			break;

		default:
//			alert(TreeNode.getKeyCode(evt));
			return;
	}
	TreeNode.clearEvent(evt);
};

JsonFx.Bindings.add(
	"a.js-ExtLink",
	function(/*DOM*/ elem) {
		elem.onclick = function(/*Event*/ evt) {
			window.open(this.href);
			return false;
		};
	},
	function(/*DOM*/ elem) {
		elem.onclick = null;
	});

/* Ajax history callback */
Example.historyCallback = function(/*object*/ info) {
	if (!info) {
		info = { slide: 0 };
	}

	Example.loadSlideInternal( info.slide );
};