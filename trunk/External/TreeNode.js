/*global JsonML, JsonFx, TreeNode */

/* dependency checking */
if ("undefined" === typeof JsonML) {
	throw new Error("TreeNode requires JsonML");
}
if ("undefined" === typeof JsonFx) {
	throw new Error("TreeNode requires JsonFx");
}
if ("undefined" === typeof JsonFx.UI) {
	throw new Error("TreeNode requires JsonFx.UI");
}
if ("undefined" === typeof JsonFx.Bindings) {
	throw new Error("TreeNode requires JsonFx.Bindings");
}

/*class TreeNode*/
if ("undefined" === typeof window.TreeNode) {
	window.TreeNode = {};
}

/*void*/ TreeNode.addSubTree = function(/*DOM*/ elem, /*object*/ data) {
	if (!data || !elem) {
		return;
	}

	// bind subtree
	var tree = TreeNode.treeJbst.dataBind(data);
	tree = JsonML.parse(tree, JsonFx.Bindings.bindOne);

	if (!elem.parentNode) {
		// can happen if elem has already been disposed
		return;
	}
	// simulate insertAfter(...)
	elem.parentNode.insertBefore(tree, elem.nextSibling);

	// show the children
	TreeNode.expand(elem);
};

/*bool*/ TreeNode.isCollapsed = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	return JsonFx.UI.hasClass(elem, "js-ClosedNode");
};

/*void*/ TreeNode.expand = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	JsonFx.UI.removeClass(elem, "js-ClosedNode");
};

/*void*/ TreeNode.collapse = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	JsonFx.UI.addClass(elem, "js-ClosedNode");
};

/*void*/ TreeNode.toggle = function(/*DOM*/ elem) {
	if (TreeNode.isCollapsed(elem)) {
		TreeNode.expand(elem);
	} else {
		TreeNode.collapse(elem);
	}
	elem.focus();
};

/*DOM*/ TreeNode.select = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findChild(elem, "js-NodeLabel");
	if (elem) {
		elem.focus();
	}
	return elem;
};

/*DOM*/ TreeNode.moveParent = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findParent(elem, "js-TreeNode", true);
	elem = TreeNode.select(elem);
	return elem;
};

/*DOM*/ TreeNode.moveChild = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findChild(elem, "js-TreeNode", true);
	elem = TreeNode.select(elem);
	return elem;
};

/*DOM*/ TreeNode.movePrev = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findPrev(elem, "js-TreeNode", true);
	elem = TreeNode.select(elem);
	return elem;
};

/*DOM*/ TreeNode.moveNext = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findNext(elem, "js-TreeNode", true);
	elem = TreeNode.select(elem);
	return elem;
};

/* Event utilities ----------------------------------------------*/

/*void*/ TreeNode.clearEvent = function(/*Event*/ evt) {
	evt = evt || window.event;
	if (evt) {
		if (evt.stopPropagation) {
			evt.stopPropagation();
			evt.preventDefault();
		} else {
			try {
				evt.cancelBubble = true;
				evt.returnValue = false;
			} catch (ex) {
				// IE6
			}
		}
	}
};

/*int*/ TreeNode.getKeyCode = function(/*Event*/ evt) {
	evt = evt || window.event;
	if (!evt) {
		return -1;
	}
	return Number(evt.keyCode || evt.charCode || -1);
};

/*void*/ TreeNode.onkeydown = function(/*Event*/ evt, /*DOM*/ elem) {
	evt = evt||window.event;

	switch (TreeNode.getKeyCode(evt)) {
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
			if (TreeNode.isCollapsed(elem)) {
				if (elem.click) {
					elem.click();
				} else if (elem.onclick) {
					elem.onclick();
				}
			} else {
				TreeNode.moveChild(elem);
			}
			break;

		default:
//			alert(TreeNode.getKeyCode(evt));
			return;
	}
	TreeNode.clearEvent(evt);
};

JsonFx.Bindings.register(
	"a",
	"js-NodeLabel",
	function(/*DOM*/ elem) {
		elem.onkeydown = function(/*Event*/ evt) {
			return TreeNode.onkeydown(evt, elem);
		};
		elem.ondblclick = function(/*Event*/ evt) {
			if (elem.click) {
				return elem.click(evt);
			}
			if (elem.onclick) {
				return elem.onclick(evt);
			}
		};
	},
	function(/*DOM*/ elem) {
		elem.onkeydown = null;
		elem.ondblclick = null;
	});

