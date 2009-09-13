/*global JsonFx, JbstUIToolkit, window */

/* namespace DemoApp.TreeView */
var DemoApp;
if ("undefined" === typeof DemoApp) {
	DemoApp = {};
}
if ("undefined" === typeof DemoApp.TreeView) {
	DemoApp.TreeView = {};
}

/*-------------------------------------------------------------------*/

// override the control methods to provide an app-specific implementation
(function() {

	/* override JbstUIToolkit.TreeView.TreeNode.getAction ------------------ */

	/*const string*/ var host = (window.location.protocol+"//"+window.location.host);

	/*void*/ function lazyLoad(/*Event*/ evt) {
		// begin perf timing
//		var start = Perf.now();

		var path = this.href||"";
		if (path.indexOf(host) === 0) {
			// DOM hrefs get fully qualified
			path = path.substr(host.length);
		}

		var elem = this;
		DemoApp.BrowseService.browse(
			path,
			{
				onSuccess : function(/*object*/ data, /*object*/ cx) {
					if (!data || !elem) {
						return;
					}

					if (data.category === "Folder") {
						// lazy loaded data is a sub tree
						elem.onclick = JbstUIToolkit.TreeView.toggle;

						JbstUIToolkit.TreeView.addSubTree(elem, data);
					}
//				},
//				onComplete : function(/*XHR*/ r, /*object*/ cx) {
//					Perf.add(Perf.now() - start);
				}
			});

		return false;
	}

	/*void*/ function loadPreview(/*Event*/ evt) {
		// begin perf timing
//		var start = Perf.now();

		var path = this.href;
		if (path.indexOf(host) === 0) {
			// DOM hrefs get fully qualified
			path = path.substr(host.length);
		}

		DemoApp.BrowseService.view(
			path,
			{
				onSuccess : function(content) {
					DemoApp.TreeView.PreviewFile.show(content);
//				},
//				onComplete : function(/*XHR*/ r, /*object*/ cx) {
//					Perf.add(Perf.now() - start);
				}
			});

		return false;
	}

	function loadImage(/*Event*/ evt) {
		// display the image
		DemoApp.TreeView.PreviewImage.show(this.href);
		return false;
	}

	function openWindow(/*Event*/ evt) {
		// open in a new window
		window.open(this.href);
		return false;
	}

	function voidEvent(/*Event*/ evt) {
		// suppress event
		return false;
	}

	// override default implementation with our custom actions
	JbstUIToolkit.TreeView.TreeNode.getAction = function(/*object*/ data) {

		// choose an action based upon category
		switch (data && data.category) {
			case "Folder":
				return lazyLoad;

			case "Code":
			case "Text":
			case "Xml":
				return loadPreview;

			case "Image":
				return loadImage;

			case "Audio":
			case "Compressed":
			case "Document":
			case "Video":
				return openWindow;

			default:
				return voidEvent;
		}
	};

	/* override JbstUIToolkit.TreeView.TreeNode.getLabelCss ------------------ */

	// create a closure containing the old method now the
	// new method can reference the old method when overriding
	var baseGetLabelCss = JbstUIToolkit.TreeView.TreeNode.getLabelCss;

	JbstUIToolkit.TreeView.TreeNode.getLabelCss = function(/*object*/ data) {
		// this syntax is made available via the closure
		var css = baseGetLabelCss(data);

		if (!data.lazyLoad) {
			return css+" Download";
		}

		if (data.category === "Folder") {
			css += " LazyLoad";
		}

		css += " "+data.category+"Label";

		var ext = data.path.lastIndexOf('.')+1;
		if (ext) {
			css += " Extension-"+data.path.substr(ext).toLowerCase();
		}

		if (data.isSpecial) {
			css += " IsSpecial";
		}
		return css;
	};

	/* override JbstUIToolkit.TreeView.TreeRoot.getChildren ------------------ */
	
	// override default implementation
	JbstUIToolkit.TreeView.TreeRoot.getChildren = function(/*object*/ data) {
		if (!data || !data.children) {
			return [];
		}

		return data.children;
	};

})();
