/*global Calendar */

/* namespace window.Calendar */
if ("undefined" === typeof window.Calendar) {
	window.Calendar = {};
}

Calendar.DateUtil = {

	filterRange : function(/*Events[]*/ events, /*Date*/ start, /*Date*/ end) {
		var items = [];

		for (var i=0; i<events.length; i++) {
			if (events[i].Starting.between(start, end) ||
				events[i].Ending.between(start, end)) {
				items.push(events[i]);
			}
		}

		return items;
	},

	filterDay : function(/*Events[]*/ events, /*Date*/ date) {
		var start = date.clone().clearTime(),
			end = date.clone().set( {
				hour: 23,
				minute: 59,
				second: 59
			});

		return Calendar.DateUtil.filterRange(events, start, end);
	}
};
