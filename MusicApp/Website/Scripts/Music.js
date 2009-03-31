/*global Music */

/* enable valid CSS to target browsers without reverting to CSS hacks */
JsonFx.UA.setCssUserAgent();

/* Music namespace */
if ("undefined" === typeof window.Music) {
	window.Music = {};
}

JsonFx.IO.Service.prototype.onBeginRequest = function() {
	// only show if takes longer than 100ms
	Music.Loading.show(100);
};

JsonFx.IO.Service.prototype.onEndRequest = function() {
	Music.Loading.hide();
};

// current data (model)
Music.curData = null;
Music.curView = null;
Music.curSort = {
	field: null,
	desc: false
};

// data binding helper method
Music.display = function(/*JSON*/ data, /*JBST*/ view, /*string*/ field){
	// "data" is just a pure JSON graph at this point
	if (data) {
		Music.curData = data;
	}
	if (view) {
		Music.curView = view;
	}

	if (!Music.curData || !Music.curView) {
		return;
	}

	var demo = document.getElementById("ExampleArea");
	if (!demo) {
		return;
	}

	// clear the demo area
	while (demo.lastChild) {
		demo.removeChild(demo.lastChild);
	}

	if (field) {
		// set to ascending if changing, or toggle if same
		Music.curSort.desc = (Music.curSort.field === field) && !Music.curSort.desc;
		Music.curSort.field = field;
		Music.curData.members = Music.sort(Music.curData.members, Music.curSort.field, Music.curSort.desc);
	}

	try {
		// bind the JBST to JSON and produce DOM
		var ui = Music.curView.bind(Music.curData);

		// display fully hydrated elements
		demo.appendChild(ui);
	} catch (ex) {
		/*jslint debug:true*/
		debugger;
		demo.appendChild(document.createTextNode("Error: "+ex.message));
	}
};

// list sorting utility
/*array*/ Music.sort = (function(){
	// define logic in closure functions
	function compare(/*object*/ a, /*object*/ b) {
		a = a[field];
		b = b[field];

		// send empty items to end
		var aIsEmpty = ("undefined" === typeof a || a === null);
		var bIsEmpty = ("undefined" === typeof b || b === null);
		if (aIsEmpty) {
			return bIsEmpty ? 0 : 1;
		}
		if (bIsEmpty) {
			return aIsEmpty ? 0 : -1;
		}

		// convert arrays to something comparable
		if (a instanceof Array) {
			a = a.join('\n');
		}
		if (b instanceof Array) {
			b = b.join('\n');
		}

		if (a < b) {
			return -1;
		}
		if (a > b) {
			return 1;
		}
		return 0;
	}

	// reverse is same logic negated
	function reverse(/*object*/ a, /*object*/ b) {
		return -compare(a, b);
	}

	return function(/*array*/ list, /*string*/ field, /*bool*/ desc) {
		if (!list || !field) {
			return list;
		}

		return list.sort(desc ? reverse : compare);
	};
})();
