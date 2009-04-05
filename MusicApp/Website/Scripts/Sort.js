/*global Music */

/*
	this is a generalized sort utility
	it maintains the current data list, field, direction
	it also generates closure callbacks for column headers
*/

/* Music namespace */
if ("undefined" === typeof window.Music) {
	window.Music = {};
}

/* Ctor */
Music.Sort = function(/*array*/ list, /*string*/ field, /*bool*/ desc) {

	// using the input args as private fields

	// private list sorting methods
	/*int*/ function compare(/*object*/ a, /*object*/ b) {
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
	/*int*/ function reverse(/*object*/ a, /*object*/ b) {
		return -compare(a, b);
	}

	/*array*/ function sort(/*string*/ field, /*bool*/ desc) {
		if (!list || !field) {
			return list;
		}

		return list.sort(desc ? reverse : compare);
	}

	/*array*/ this.getList = function() {
		return list;
	};

	// generates a closure which will sort the data and update the current state
	/*function*/ this.sortClosure = function(/*string*/ fld) {
		// this can be called repeatedly
		return function() {
			// update the internal sort state
			desc = (field === fld) && !desc;
			field = fld;

			// sort in place affecting all other closures built from this object
			sort(fld, desc);
		};
	};

	if (field) {
		sort(field, desc);
	}
};
