/*global JsonFx, JbstUIToolkit, window */
/*
	Browse.js

	Controller for the file browse views
*/

/*
	it is a best practice to not clutter the global namespace
	creating top level objects which contain variables and functions
	allows us to simulate namespaces
*/

/* namespace DemoApp */
var DemoApp;
if ("undefined" === typeof DemoApp) {
	DemoApp = {};
}

/*-------------------------------------------------------------------*/

/*void*/ DemoApp.display = function(/*string*/ data) {

	var preview = DemoApp.previewFile.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};

/*void*/ DemoApp.imageDisplay = function(/*string*/ data) {
	var preview = DemoApp.previewImage.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};

/*void*/ DemoApp.loadComplete = function(/*object*/ data, /*object*/ cx) {
	if (!data) {
		return;
	}

	var elem = cx && cx.elem;
	if (elem && data.category === "Folder") {
		// lazy loaded data is a sub tree
		elem.onclick = function(/*Event*/ evt) {
			TreeNode.toggle(elem);
			return false;
		};

//		elem.focus();
		return TreeNode.addSubTree(elem, data);
	}
};

/*void*/ DemoApp.loadError = function (/*object*/ result, /*object*/ cx, /*Error*/ ex) {
	var msg = ex.message || ex.description || ex;
	if (msg && window.confirm(msg)) {
		/*jslint debug:true */
		debugger;
		/*jslint debug:false */
	}
};

/*const string*/ DemoApp.host = (window.location.protocol+"//"+window.location.host);

/*void*/ DemoApp.lazyLoad = function(/*DOM*/ elem) {
	if (!elem) {
		return;
	}

	// begin perf timing
//	var start = Perf.now();

	var path = elem.href||"";
	if (path.indexOf(DemoApp.host) === 0) {
		// DOM hrefs get fully qualified
		path = path.substr(DemoApp.host.length);
	}

	DemoApp.BrowseServiceProxy.browse(
		path,
		{
			onSuccess : DemoApp.loadComplete,
			onFailure : DemoApp.loadError,
			onComplete : function(/*XHR*/ r, /*object*/ cx) {
//				Perf.add(Perf.now() - start);
			},
			context : { elem: elem }
		});
};

/*void*/ DemoApp.loadPreview = function(/*DOM*/ elem) {
	if (!elem) {
		return;
	}

	// begin perf timing
//	var start = Perf.now();

	var path = elem.href;
	if (path.indexOf(DemoApp.host) === 0) {
		// DOM hrefs get fully qualified
		path = path.substr(DemoApp.host.length);
	}

	DemoApp.BrowseServiceProxy.view(
		path,
		{
			onSuccess : DemoApp.display,
			onFailure : DemoApp.loadError,
			onComplete : function(/*XHR*/ r, /*object*/ cx) {
//				Perf.add(Perf.now() - start);
			},
			context : { elem: elem }
		});
};

/*void*/ DemoApp.init = function(/*DOM*/ elem) {
	// begin perf timing
//	var start = Perf.now();

	var browseData = JsonFx.UI.findChild(elem, "js-BrowseData");
	var data = browseData&&(browseData.value||browseData.textContent||browseData.innerText);
	if (!data) {
		DemoApp.lazyLoad(elem);
		return;
	}
	data = JSON.parse(data, JsonFx.jsonReviver);

	TreeNode.addSubTree(elem, data);

	// select the first node
	TreeNode.select(document.body);

	DemoApp.setPageTitle(data.name);

//	Perf.add(Perf.now() - start);
};

(function() {
	// override the template to provide a data specific binding
	// create a closure containing the old method using a familiar syntax
	// now the new method can reference the old method when overriding
	var base = { getLabelCss: JbstUIToolkit.TreeView.TreeNode.getLabelCss };

	JbstUIToolkit.TreeView.TreeNode.getLabelCss = function(/*object*/ data) {
		// this syntax is made available via the closure
		var css = base.getLabelCss(data);

		if (!data.lazyLoad) {
			return css+" Download";
		}

		// using JsonFx behavior bindings to
		// automatically hookup appropriate actions
		switch (data.category) {
			case "Folder":
				css += " LazyLoad js-LazyLoad";
				break;
			case "Code":
			case "Text":
			case "Xml":
				css += " js-FilePreview";
				break;
			case "Image":
				css += " js-ImagePreview";
				break;
			case "Audio":
			case "Compressed":
			case "Document":
			case "Video":
				css += " js-ExtLink";
				break;
			default:
				css += " js-Void";
				break;
		}

		css += " "+data.category+"Label";

		var ext = data.path.substr(data.path.lastIndexOf('.')+1);
		if (ext) {
			css += " Extension-"+ext.toLowerCase();
		}

		if (data.isSpecial) {
			css += " IsSpecial";
		}
		return css;
	};
})();

(function() {
	// override the template to provide a data specific binding
	// create a closure containing the old method using a familiar syntax
	// now the new method can reference the old method when overriding
	var base = { getChildren: JbstUIToolkit.TreeView.TreeRoot.getChildren };

	JbstUIToolkit.TreeView.TreeRoot.getChildren = function(/*object*/ data) {
		if (!data) {
			// this syntax is made available via the closure
			return base.getChildren(data);
		}

		return data.children;
	};
})();

JsonFx.Bindings.add(
	"p.js-BrowseRoot",
	DemoApp.init,
	null);

JsonFx.Bindings.add(
	"a.js-LazyLoad",
	function(/*DOM*/ elem) {
		elem.onclick = function(/*Event*/ evt) {
			DemoApp.lazyLoad(elem);
			return false;
		};
	},
	function(/*DOM*/ elem) {
		elem.data = null;
		elem.onclick = null;
	});

JsonFx.Bindings.add(
	"a.js-FilePreview",
	function(/*DOM*/ elem) {
		elem.onclick = function(/*Event*/ evt) {
			DemoApp.loadPreview(elem);
			return false;
		};
	},
	function(/*DOM*/ elem) {
		elem.data = null;
		elem.onclick = null;
	});

JsonFx.Bindings.add(
	"a.js-ImagePreview",
	function(/*DOM*/ elem) {
		elem.onclick = function(/*Event*/ evt) {
			DemoApp.imageDisplay(elem.href);
			return false;
		};
	},
	function(/*DOM*/ elem) {
		elem.data = null;
		elem.onclick = null;
	});

JsonFx.Bindings.add(
	"a.js-Void",
	function(/*DOM*/ elem) {
		elem.onclick = function(/*Event*/ evt) {
			return false;
		};
	},
	function(/*DOM*/ elem) {
		elem.data = null;
		elem.onclick = null;
	});
