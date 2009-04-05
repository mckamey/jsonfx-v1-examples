/*global Music, $ */

/*
	this is core logic for edit events and actions
*/

/* Music namespace */
if ("undefined" === typeof window.Music) {
	window.Music = {};
}

/* Ctor */
Music.MemberEdit = {

	keyup: function(/*event*/ evt) {
		evt = evt || window.event;

		if (evt.keyCode === 0x1B /*ESC*/) {
			// find the cancel button and click it
			$(this).find("a.cancel").click();

		} else if (evt.keyCode === 0x0D /*ENTER*/) {
			// find the save button and click it
			$(this).find("a.save").click();
		}
	},

	cancelBubble: function(/*event*/ evt) {
		evt = evt || window.event;
		if (evt.stopPropagation) {
			evt.stopPropagation();
		} else {
			try {
				evt.cancelBubble = true;
			} catch (ex) {
				// IE6
			}
		}
	},

	focusField: function() {
		// select the first field
		$(this).find(":text").slice(0,1).focus();
	},

	initText: function() {
		$(this).focus(function() {
			// hilite the field on focus
			this.select();
		});
	},

	initYear: function() {
		var self = $(this);
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
		Music.MemberEdit.initText.call(this);
	},

	closureEdit: function(/*JBST*/ template, /*Member*/ member, /*int*/ index, /*int*/ count) {
		// generates a closure which maintains a reference to
		// the originally bound data and the target template
		return function() {
			var elem = template.bind(member, index, count);
			$(this).parents(".member").replaceWith(elem);

			return false;
		};
	},

	closureSave: function(/*JBST*/ template, /*Member*/ member, /*int*/ index, /*int*/ count) {
		// generates a closure which maintains a reference to
		// the originally bound data and the target template
		return function() {
			var form = $(this.form);

			// apply fields to the data item
			form.find(":text").each(function() {
					member[this.name] = $(this).val();

					switch(this.name) {
						case "ArtistID":
						case "MemberID":
						case "StartYear":
						case "EndYear":
							member[this.name] = member[this.name] ?
								Number(member[this.name]) :
								null;
							break;
					}
				});

			form.find("textarea").each(function() {
					member[this.name] = $(this).val().split(/\s*[\n\r,;\/]+\s*/).join(',');
				});

			// artist was stored on the table, grab a reference
			var artist = $(this).parents(".members")[0].artist;

			var old = $(this).parents(".member");
			Music.Service.saveMember(
				member,
				{
					onSuccess: function(member) {
						// add the saved member to the artist data
						// so view changes and sorts reflect the addition
						artist.Members.unshift(member);

						// rebind member and replace form
						var elem = template.bind(member, index, count);
						old.replaceWith(elem);
					}
				});

			return false;
		};
	},

	closureCancel: function(/*JBST*/ template, /*Member*/ member, /*int*/ index, /*int*/ count) {
		// generates a closure which maintains a
		// reference to the originally bound data
		return function() {
			if (!member.MemberID) {
				// user was adding a new member
				// so just remove form
				$(this).parents(".member").removeFade();
				return false;
			}

			// user was editing an existing member
			// so rebind original data and replace form
			var elem = template.bind(member, index, count);
			$(this).parents(".member").replaceWith(elem);

			return false;
		};
	},

	closureDelete: function(/*JBST*/ template, /*Member*/ member) {
		// generates a closure which maintains a
		// reference to the originally bound data
		return function() {
			var button = $(this);

			Music.Confirm.show(
				/* message */
				"Are you sure you want to delete \""+member.FirstName+" "+member.LastName+"\"?",

				/* OK action */
				function() {
					// artist was stored on the table, grab a reference
					var artist = button.parents(".members")[0].artist;

					var editForm = button.parents(".member");
					Music.Service.deleteMember(
						member.MemberID,
						{
							onSuccess: function(/*bool*/ result) {
								var fixZebra = false;
							
								// add the saved member to the artist data
								// so view changes and sorts reflect the addition
								for (var i=0; i<artist.Members.length; i++) {
									if (artist.Members[i].MemberID === member.MemberID) {
										// remove the single member
										artist.Members.splice(i, 1);
										if (i !== 0 && i !== artist.Members.length) {
											fixZebra = true;
										}
									}
								}

								// remove edit form
								if (fixZebra) {
									var grid = editForm.parents(".artist");
									editForm.removeFade(300, function() {
										// fix zebra-stripes by rebinding entire grid
										var view = template.bind(artist);
										grid.replaceWith(view);
									});
								} else {
									editForm.removeFade();
								}
							}
						});
				},

				/* Cancel action */
				null,

				/* OK label */
				"Delete",

				/* Cancel label */
				"Cancel");

			return false;
		};
	}
};