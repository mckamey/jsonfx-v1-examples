/*global JSON, JsonML, JsonFx, Example, TreeNode */
/*
	Browse.js

	File browser script
*/

/*
	it is a best practice to not clutter the global namespace
	creating top level objects which contain variables and functions
	allows us to simulate namespaces
*/

/* namespace Example */
if ("undefined" === typeof window.Example) {
	window.Example = {};
}

/*-------------------------------------------------------------------*/

/*void*/ Example.display = function(/*object*/ data) {
	switch (data.category)
	{
		case "Code":
		case "Document":
		case "Text":
		case "Web":
		case "Xml":
			// TODO: implement viewer
			alert(data.category);
			break;
		default:
			// TODO: implement viewer
			alert(JSON.stringify(data));
			break;
	}
};

/*void*/ Example.loadComplete = function(/*object*/ data, /*object*/ cx) {
	if (!data) {
		return;
	}

	var elem = cx && cx.elem;
	if (data.category === "Folder") {
		// lazy loaded data is a sub tree
		elem.onclick = function(/*Event*/ evt) {
			TreeNode.toggle(elem);
			return false;
		};
		
		elem.focus();
		return TreeNode.addSubTree(elem, data);
	}

	// lazy loaded data is a leaf node
	if (elem) {
		elem.onclick = function(/*Event*/ evt) {
			Example.display(data);
			return false;
		};
	}

	Example.display(data);
};

/*void*/ Example.loadError = function (/*object*/ result, /*object*/ cx, /*Error*/ ex) {
	var msg = ex.message || ex.description || ex;
	if (msg && window.confirm(msg)) {
		/*jslint evil: true */
		debugger;
	}
};

/*const string*/ Example.host = (window.location.protocol+"//"+window.location.host);

/*void*/ Example.lazyLoad = function(/*DOM*/ elem) {
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

/*void*/ Example.init = function(/*DOM*/ elem) {
	// begin perf timing
//	var start = Perf.now();

	var browseData = JsonFx.DOM.findChild(elem, "js-BrowseData");
	var data = browseData&&(browseData.value||browseData.textContent||browseData.innerText);
	if (!data) {
		// TODO: replace this
		elem.href = "/";
		Example.lazyLoad(elem);
		return;
	}
	data = JSON.parse(data, JsonFx.IO.jsonReviver);

	var tree = TreeNode.addSubTree(elem, data);

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

		css += " LazyLoad js-LazyLoad";

		var extension = (data.category === "Folder") ?
			"" :
			data.path.substr(data.path.lastIndexOf('.')+1);

		css += " "+data.category+"Label "+"Extension-"+extension;

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

JsonFx.Bindings.register(
	"p",
	"js-BrowseRoot",
	Example.init,
	null);

JsonFx.Bindings.register(
	"a",
	"js-LazyLoad",
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
