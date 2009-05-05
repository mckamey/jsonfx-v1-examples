/*global Music, $ */

/*
	field event handlers
*/

/* Music namespace */
if ("undefined" === typeof window.Music) {
	window.Music = {};
}

/* Ctor */
Music.Fields = {

	/* event handler for edit form */
	keyUp: function(/*event*/ evt) {
		evt = evt || window.event;

		if (evt.keyCode === 0x1B /*ESC*/) {
			// find the cancel button and click it
			$(this).find("a.cancel").click();

		} else if (evt.keyCode === 0x0D /*ENTER*/) {
			// find the save button and click it
			$(this).find("a.save").click();
		}
	},// keyUp

	/* event handler for textareas */
	cancelBubble: function(/*event*/ evt) {
		evt = evt || window.event;

		// prevent event from bubbling up to form
		// so that textareas can have newlines without
		// submitting forms
		if (evt.stopPropagation) {
			evt.stopPropagation();
		} else {
			try {
				evt.cancelBubble = true;
			} catch (ex) {
				// IE6
			}
		}
	},// cancelBubble

	/* focuses the first form field */
	focusField: function() {

		// select the first field
		$(this).find(":text").slice(0,1).focus();
	},// focusField

	/* initialization for text inputs */
	initText: function() {
		// attach onfocus handler
		$(this).focus(function() {
			// hilite the field on focus
			this.select();
		});
	},// initText

	/* initialization for year inputs */
	initYear: function() {
		var self = $(this);

		// attach onchange handler
		self.change(function() {
			// force into correct data type (16-bit integer)
			var year = self.val().replace(/[^0-9]/, "");
			if (!isFinite(year) || year < -32768 || year > 32767) {
				self.val("");
			} else {
				self.val(year);
			}
		});

		// calling with current element as "this" context
		Music.Fields.initText.call(this);
	}// initYear
};