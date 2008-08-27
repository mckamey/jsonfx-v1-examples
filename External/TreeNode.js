/*global JsonFx, TreeNode */

/* dependency checking */
if ("undefined" === typeof JsonFx) {
	throw new Error("TreeNode requires JsonFx");
}
if ("undefined" === typeof JsonFx.DOM) {
	throw new Error("TreeNode requires JsonFx.DOM");
}
if ("undefined" === typeof JsonFx.Bindings) {
	throw new Error("TreeNode requires JsonFx.Bindings");
}

/* namespace TreeNode */
if ("undefined" === typeof TreeNode) {
	window.TreeNode = {};
}

/*void*/ TreeNode.isCollapsed = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findParent(elem, "js-Node");
	return JsonFx.DOM.hasClass(elem, "js-BrowseLeaf") ||
		JsonFx.DOM.hasClass(elem, "js-ClosedNode");
};

/*void*/ TreeNode.expand = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findParent(elem, "js-Node");
	JsonFx.DOM.removeClass(elem, "js-ClosedNode");
};

/*void*/ TreeNode.collapse = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findParent(elem, "js-Node");
	JsonFx.DOM.addClass(elem, "js-ClosedNode");
};

/*void*/ TreeNode.toggle = function(/*DOM*/ elem) {
	if (TreeNode.isCollapsed(elem)) {
		TreeNode.expand(elem);
	} else {
		TreeNode.collapse(elem);
	}
};

/*DOM*/ TreeNode.select = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findChild(elem, "js-NodeLabel");
	if (elem) {
		elem.focus();
	}
	return elem;
};

/*DOM*/ TreeNode.moveParent = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findParent(elem, "js-Node");
	elem = JsonFx.DOM.findParent(elem, "js-Node", true);
	elem = TreeNode.select(elem);
	return elem;
};

/*DOM*/ TreeNode.moveChild = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findParent(elem, "js-Node");
	elem = JsonFx.DOM.findChild(elem, "js-Node", true);
	elem = TreeNode.select(elem);
	return elem;
};

/*DOM*/ TreeNode.movePrev = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findParent(elem, "js-Node");
	elem = JsonFx.DOM.findPrev(elem, "js-Node", true);
	elem = TreeNode.select(elem);
	return elem;
};

/*DOM*/ TreeNode.moveNext = function(/*DOM*/ elem) {
	elem = JsonFx.DOM.findParent(elem, "js-Node");
	elem = JsonFx.DOM.findNext(elem, "js-Node", true);
	elem = TreeNode.select(elem);
	return elem;
};

/*void*/ TreeNode.onkeydown = function(/*Event*/ evt, /*DOM*/ elem) {
	evt = evt||window.event;

	switch (JsonFx.DOM.getKeyCode(evt)) {
		case 0x0D: // enter
		case 0x20: // space
			if (elem.click) {
				elem.click();
			} else if (elem.onclick) {
				elem.onclick();
			}
			break;
		case 0x25: // left arrow
			if (!TreeNode.isCollapsed(elem)) {
				TreeNode.collapse(elem);
			} else {
				TreeNode.moveParent(elem);
			}
			break;
		case 0x28: // down arrow
			if (!TreeNode.isCollapsed(elem)) {
				TreeNode.moveChild(elem);
			} else {
				while (elem && !TreeNode.moveNext(elem)) {
					elem = TreeNode.moveParent(elem);
				}
			}
			break;
		case 0x26: // up arrow
			var prev = TreeNode.movePrev(elem);
			if (!prev) {
				TreeNode.moveParent(elem);
			} else if (!TreeNode.isCollapsed(prev)) {
				prev = TreeNode.moveChild(prev);
				while (prev) {
					prev = TreeNode.moveNext(prev);
				}
			}
			break;
		case 0x27: // right arrow
			if (JsonFx.DOM.hasClass(elem, "js-LazyLoad")) {
				if (elem.click) {
					elem.click();
				} else if (elem.onclick) {
					elem.onclick();
				}
			} else if (TreeNode.isCollapsed(elem)) {
				TreeNode.expand(elem);
			} else {
				TreeNode.moveChild(elem);
			}
			break;
		default:
//			alert(JsonFx.DOM.getKeyCode(evt));
			return;
	}
	JsonFx.DOM.clearEvent(evt);
};

JsonFx.Bindings.register(
	"a",
	"js-NodeLabel",
	function(/*DOM*/ elem) {
		elem.onkeydown = function(/*Event*/ evt) {
			return TreeNode.onkeydown(evt, elem);
		};
	},
	function(/*DOM*/ elem) {
		elem.onkeydown = null;
	});
