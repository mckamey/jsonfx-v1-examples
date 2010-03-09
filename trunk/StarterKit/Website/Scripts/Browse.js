/*global JSON, JsonML, JsonFx, TreeNode, window */
/*
	Browse.js

	Controller for the file browse views
*/

/*
	it is a best practice to not clutter the global namespace
	creating top level objects which contain variables and functions
	allows us to simulate namespaces
*/

/* namespace Example */
var Example;
if ("undefined" === typeof Example) {
	Example = {};
}

/*-------------------------------------------------------------------*/

/*void*/ Example.display = function(/*string*/ data) {

	var preview = Example.previewFile.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};

/*void*/ Example.imageDisplay = function(/*string*/ data) {
	var preview = Example.previewImage.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};

/*void*/ Example.loadComplete = function(/*object*/ data, /*object*/ cx) {
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

/*void*/ Example.loadError = function (/*object*/ result, /*object*/ cx, /*Error*/ ex) {
	var msg = ex.message || ex.description || ex;
	if (msg && window.confirm(msg)) {
		/*jslint debug:true */
		debugger;
		/*jslint debug:false */
	}
};

/*const string*/ Example.host = (location.protocol+"//"+location.host);

/*void*/ Example.lazyLoad = function(/*DOM*/ elem) {
	if (!elem) {
		return;
	}

	// begin perf timing
//	var start = Perf.now();

	var path = elem.href||"";
	if (path.indexOf(Example.host) === 0) {
		// DOM hrefs get fully qualified
		path = path.substr(Example.host.length);
	}

	Example.BrowseServiceProxy.browse(
		path,
		{
			onSuccess : Example.loadComplete,
			onFailure : Example.loadError,
			onComplete : function(/*XHR*/ r, /*object*/ cx) {
//				Perf.add(Perf.now() - start);
			},
			context : { elem: elem }
		});
};

/*void*/ Example.loadPreview = function(/*DOM*/ elem) {
	if (!elem) {
		return;
	}

	// begin perf timing
//	var start = Perf.now();

	var path = elem.href;
	if (path.indexOf(Example.host) === 0) {
		// DOM hrefs get fully qualified
		path = path.substr(Example.host.length);
	}

	Example.BrowseServiceProxy.view(
		path,
		{
			onSuccess : Example.display,
			onFailure : Example.loadError,
			onComplete : function(/*XHR*/ r, /*object*/ cx) {
//				Perf.add(Perf.now() - start);
			},
			context : { elem: elem }
		});
};

/*void*/ Example.init = function(/*DOM*/ elem) {
	// begin perf timing
//	var start = Perf.now();

	var browseData = JsonFx.UI.findChild(elem, "js-BrowseData");
	var data = browseData&&(browseData.value||browseData.textContent||browseData.innerText);
	if (!data) {
		Example.lazyLoad(elem);
		return;
	}
	data = JSON.parse(data, JsonFx.jsonReviver);

	TreeNode.addSubTree(elem, data);

	// select the first node
	TreeNode.select(document.body);

	Example.setPageTitle(data.name);

//	Perf.add(Perf.now() - start);
};

(function() {
	// override the template to provide a data specific binding
	// create a closure containing the old method using a familiar syntax
	// now the new method can reference the old method when overriding
	var base = { getLabelCss: TreeNode.nodeJbst.getLabelCss };

	TreeNode.nodeJbst.getLabelCss = function(/*object*/ data) {
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
	var base = { getChildren: TreeNode.treeJbst.getChildren };

	TreeNode.treeJbst.getChildren = function(/*object*/ data) {
		if (!data) {
			// this syntax is made available via the closure
			return base.getChildren(data);
		}

		return data.children;
	};
})();

JsonFx.Bindings.add(
	"p.js-BrowseRoot",
	Example.init,
	null);

JsonFx.Bindings.add(
	"a.js-LazyLoad",
	function(/*DOM*/ elem) {
		elem.onclick = function(/*Event*/ evt) {
			Example.lazyLoad(elem);
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
			Example.loadPreview(elem);
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
			Example.imageDisplay(elem.href);
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
