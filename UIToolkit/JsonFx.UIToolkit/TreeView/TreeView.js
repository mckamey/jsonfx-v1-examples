/*global JsonFx, window */

/* dependency checking */
if ("undefined" === typeof JsonFx) {
	throw new Error("TreeNode requires JsonFx");
}
if ("undefined" === typeof JsonFx.UI) {
	throw new Error("TreeNode requires JsonFx.UI");
}
if ("undefined" === typeof JsonFx.Bindings) {
	throw new Error("TreeNode requires JsonFx.Bindings");
}

/* namespace JbstUIToolkit.TreeView */
var JbstUIToolkit;
if ("undefined" === typeof JbstUIToolkit) {
	JbstUIToolkit = {};
}
if ("undefined" === typeof JbstUIToolkit.TreeView) {
	JbstUIToolkit.TreeView = {};
}

/* public methods ----------------------------------------------*/

/*void*/ JbstUIToolkit.TreeView.addSubTree = function(/*DOM*/ elem, /*object*/ data) {
	if (!data || !elem) {
		return;
	}

	// bind subtree template with data
	var tree = JbstUIToolkit.TreeView.TreeRoot.bind(data);

	if (!elem.parentNode) {
		// can happen if elem has already been disposed
		return;
	}

	// simulate insertAfter(...)
	elem.parentNode.insertBefore(tree, elem.nextSibling);

	// show the children
	JbstUIToolkit.TreeView.expand.call(elem);
};

/*bool*/ JbstUIToolkit.TreeView.isCollapsed = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	return JsonFx.UI.hasClass(elem, "js-ClosedNode");
};

/*void*/ JbstUIToolkit.TreeView.expand = function(/*Event*/ evt) {
	var elem = JsonFx.UI.findParent(this, "js-TreeNode");
	JsonFx.UI.removeClass(elem, "js-ClosedNode");

	return false;
};

/*void*/ JbstUIToolkit.TreeView.collapse = function(/*Event*/ evt) {
	var elem = JsonFx.UI.findParent(this, "js-TreeNode");
	JsonFx.UI.addClass(elem, "js-ClosedNode");

	return false;
};

/*void*/ JbstUIToolkit.TreeView.toggle = function(/*Event*/ evt) {
	if (JbstUIToolkit.TreeView.isCollapsed(this)) {
		JbstUIToolkit.TreeView.expand.call(this);
	} else {
		JbstUIToolkit.TreeView.collapse.call(this);
	}
	this.focus();

	return false;
};
