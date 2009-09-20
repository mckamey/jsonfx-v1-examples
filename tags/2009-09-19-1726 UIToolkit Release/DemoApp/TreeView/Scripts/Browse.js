/*global window, JsonFx, UIT, Perf, DemoApp */

// override TreeView methods to provide an app-specific implementation

/* override UIT.TreeView.getChildren ------------------ */

// override default implementation
UIT.TreeView.getChildren = function(/*object*/ data) {
	if (!data || !data.children) {
		return null;
	}

	return data.children;
};

/* override UIT.TreeNode.getIconCSS ------------------ */

UIT.TreeView.getIconCSS = function(/*object*/ data) {
	var css = "category-"+data.category.toLowerCase();

	var ext = data.path.lastIndexOf('.')+1;
	if (ext) {
		css += " extension-"+data.path.substr(ext).toLowerCase();
	}

	return css;
};

/* override UIT.TreeNode.getName ------------------ */

// override default implementation
UIT.TreeView.getName = function(/*object*/ data) {
	if (!data || !data.name) {
		return null;
	}

	return data.name;
};

/* override UIT.TreeNode.getPath ------------------ */

// override default implementation
UIT.TreeView.getPath = function(/*object*/ data) {
	if (!data || !data.path) {
		return null;
	}

	return data.path;
};

/* override UIT.TreeNode.getAction ------------------ */
(function() {
	/*const string*/ var host = (window.location.protocol+"//"+window.location.host);

	// lazy load action (for folders)
	/*void*/ function lazyLoad(/*Event*/ evt) {
		var elem = this;
		var path = elem.href||"";
		if (path.indexOf(host) === 0) {
			// DOM hrefs get fully qualified
			path = path.substr(host.length);
		}

		UIT.Loading.show(200);

		// begin perf timing
		var start = Perf.now();

		DemoApp.BrowseService.browse(
			path,
			{
				onSuccess : function(/*object*/ data, /*object*/ cx) {
					if (!data || !elem) {
						return;
					}

					if (data.category === "Folder") {
						// lazy loaded data is a sub tree
						elem.onclick = UIT.TreeView.toggle;

						UIT.TreeView.addSubTree(elem, data);
					}
				},
				onComplete : function(/*XHR*/ r, /*object*/ cx) {
					Perf.add(Perf.now() - start);
					UIT.Loading.hide();
					Perf.refresh();
				}
			});

		UIT.TreeNode.select(elem);
		return false;
	}

	// preview action (for text)
	/*void*/ function loadPreview(/*Event*/ evt) {
		var elem = this;
		var path = elem.href||"";
		if (path.indexOf(host) === 0) {
			// DOM hrefs get fully qualified
			path = path.substr(host.length);
		}

		UIT.Loading.show(200);

		// begin perf timing
		var start = Perf.now();

		DemoApp.BrowseService.view(
			path,
			{
				onSuccess : function(content) {
					DemoApp.TreeView.PreviewFile.show(content);
				},
				onComplete : function(/*XHR*/ r, /*object*/ cx) {
					Perf.add(Perf.now() - start);
					UIT.Loading.hide();
					Perf.refresh();
				}
			});

		return false;
	}

	// preview action (for images)
	function loadImage(/*Event*/ evt) {
		// display the image
		DemoApp.TreeView.PreviewImage.show(this.href);
		return false;
	}

	// download action (for documents)
	function download(/*Event*/ evt) {
		// open in a new window
		window.open(this.href);
		return false;
	}

	// noop action (for unknown files)
	function closePreview(/*Event*/ evt) {
		// close any previews
		DemoApp.TreeView.PreviewFile.hide();
		return false;
	}

	// override default implementation with our custom actions
	UIT.TreeView.getAction = function(/*object*/ data) {
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
				return download;

			default:
				return closePreview;
		}
	};

})();
