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
	JbstUIToolkit.TreeView.expand(elem);
};

/*bool*/ JbstUIToolkit.TreeView.isCollapsed = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	return JsonFx.UI.hasClass(elem, "js-ClosedNode");
};

/*void*/ JbstUIToolkit.TreeView.expand = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	JsonFx.UI.removeClass(elem, "js-ClosedNode");
};

/*void*/ JbstUIToolkit.TreeView.collapse = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	JsonFx.UI.addClass(elem, "js-ClosedNode");
};

/*void*/ JbstUIToolkit.TreeView.toggle = function(/*DOM*/ elem) {
	if (JbstUIToolkit.TreeView.isCollapsed(elem)) {
		JbstUIToolkit.TreeView.expand(elem);
	} else {
		JbstUIToolkit.TreeView.collapse(elem);
	}
	elem.focus();
};

/*DOM*/ JbstUIToolkit.TreeView.select = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findChild(elem, "js-NodeLabel");
	if (elem) {
		elem.focus();
	}
	return elem;
};

/*DOM*/ JbstUIToolkit.TreeView.moveParent = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findParent(elem, "js-TreeNode", true);
	elem = JbstUIToolkit.TreeView.select(elem);
	return elem;
};

/*DOM*/ JbstUIToolkit.TreeView.moveChild = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findChild(elem, "js-TreeNode", true);
	elem = JbstUIToolkit.TreeView.select(elem);
	return elem;
};

/*DOM*/ JbstUIToolkit.TreeView.movePrev = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findPrev(elem, "js-TreeNode", true);
	elem = JbstUIToolkit.TreeView.select(elem);
	return elem;
};

/*DOM*/ JbstUIToolkit.TreeView.moveNext = function(/*DOM*/ elem) {
	elem = JsonFx.UI.findParent(elem, "js-TreeNode");
	elem = JsonFx.UI.findNext(elem, "js-TreeNode", true);
	elem = JbstUIToolkit.TreeView.select(elem);
	return elem;
};

/* Event utilities ----------------------------------------------*/

/*void*/ JbstUIToolkit.TreeView.clearEvent = function(/*Event*/ evt) {
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

/*int*/ JbstUIToolkit.TreeView.getKeyCode = function(/*Event*/ evt) {
	evt = evt || window.event;
	if (!evt) {
		return -1;
	}
	return Number(evt.keyCode || evt.charCode || -1);
};

/*void*/ JbstUIToolkit.TreeView.onkeydown = function(/*Event*/ evt, /*DOM*/ elem) {
	evt = evt || window.event;

	switch (JbstUIToolkit.TreeView.getKeyCode(evt)) {
		case 0x0D: // enter
		case 0x20: // space
			if (elem.click) {
				elem.click();
			} else if (elem.onclick) {
				elem.onclick();
			}
			break;

		case 0x25: // left arrow
			if (!JbstUIToolkit.TreeView.isCollapsed(elem)) {
				JbstUIToolkit.TreeView.collapse(elem);
			} else {
				JbstUIToolkit.TreeView.moveParent(elem);
			}
			break;

		case 0x28: // down arrow
			if (!JbstUIToolkit.TreeView.isCollapsed(elem)) {
				JbstUIToolkit.TreeView.moveChild(elem);
			} else {
				while (elem && !JbstUIToolkit.TreeView.moveNext(elem)) {
					elem = JbstUIToolkit.TreeView.moveParent(elem);
				}
			}
			break;

		case 0x26: // up arrow
			var prev = JbstUIToolkit.TreeView.movePrev(elem);
			if (!prev) {
				JbstUIToolkit.TreeView.moveParent(elem);
			} else if (!JbstUIToolkit.TreeView.isCollapsed(prev)) {
				prev = JbstUIToolkit.TreeView.moveChild(prev);
				while (prev) {
					prev = JbstUIToolkit.TreeView.moveNext(prev);
				}
			}
			break;

		case 0x27: // right arrow
			if (JbstUIToolkit.TreeView.isCollapsed(elem)) {
				if (elem.click) {
					elem.click();
				} else if (elem.onclick) {
					elem.onclick();
				}
			} else {
				JbstUIToolkit.TreeView.moveChild(elem);
			}
			break;

		default:
//			alert(JbstUIToolkit.TreeView.getKeyCode(evt));
			return;
	}
	JbstUIToolkit.TreeView.clearEvent(evt);
};

JsonFx.Bindings.add(
	"a.js-NodeLabel",
	function(/*DOM*/ elem) {
		elem.onkeydown = function(/*Event*/ evt) {
			return JbstUIToolkit.TreeView.onkeydown(evt, elem);
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

