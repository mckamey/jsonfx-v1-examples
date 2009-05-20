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
			end = start.clone().addDays(1).addMilliseconds(-1);

		return Calendar.DateUtil.filterRange(events, start, end);
	},

	filterHour : function(/*Events[]*/ events, /*Date*/ date) {
		var start = date.clone().set( {
				minute: 0,
				second: 0,
				milliseconds: 0
			} ),
			end = start.clone().addHours(1).addMilliseconds(-1);

		return Calendar.DateUtil.filterRange(events, start, end);
	}
};
