/*global Music, $ */

/*
	this is core logic for edit events and actions
*/

/* Music namespace */
if ("undefined" === typeof window.Music) {
	window.Music = {};
}

/* singleton */
Music.MemberEdit = {
	/* splitter for instrument delimiters */
	delim: /\s*[\n\r,;\/]+\s*/g,

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to edit buttons */
	closureEdit: function(/*JBST*/ template, /*Member*/ member, /*int*/ index, /*int*/ count) {

		// this will be the edit button onclick handler
		return function() {
			var elem = template.bind(member, index, count);
			$(this).parents(".view-item").replaceWith(elem);

			return false;
		};
	},// closureEdit

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to save buttons */
	closureSave: function(/*JBST*/ template, /*Member*/ member, /*int*/ index, /*int*/ count) {

		// this will be the save button onclick handler
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
					member[this.name] = $(this).val().split(Music.MemberEdit.delim).join(',');
				});

			// artist was stored on the table, grab a reference
			var artist = $(this).parents(".view")[0].artist;
			var isNew = !member.MemberID;

			var old = $(this).parents(".view-item");
			Music.Service.saveMember(
				member,
				{
					// this is the callback from the JSON-RPC service
					onSuccess: function(member) {
						if (isNew) {
							// add the saved member to the artist data
							// so view changes and sorts reflect the addition
							artist.Members.unshift(member);
						}

						// rebind member and replace form
						var elem = template.bind(member, index, count);
						old.replaceWith(elem);
					}
				});

			return false;
		};
	},// closureSave

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to cancel buttons */
	closureCancel: function(/*JBST*/ template, /*Member*/ member, /*int*/ index, /*int*/ count) {

		// this will be the cancel button onclick handler
		return function() {
			if (!member.MemberID) {
				// user was adding a new member
				// so just remove form
				$(this).parents(".view-item").removeFade();
				return false;
			}

			// user was editing an existing member
			// so rebind original data and replace form
			var elem = template.bind(member, index, count);
			$(this).parents(".view-item").replaceWith(elem);

			return false;
		};
	},// closureCancel

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to delete buttons */
	closureDelete: function(/*JBST*/ template, /*Member*/ member) {

		// this will be the delete button onclick handler
		return function() {
			var button = $(this);

			Music.Confirm.show(
				/* message */
				"Are you sure you want to delete \""+member.FirstName+" "+member.LastName+"\"?",

				/* OK action */
				function() {
					// artist was stored on the table, grab a reference
					var artist = button.parents(".view")[0].artist;

					var editForm = button.parents(".view-item");
					Music.Service.deleteMember(
						member.MemberID,
						{
							// this is the callback from the JSON-RPC service
							onSuccess: function() {
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
									var grid = editForm.parents(".panel");
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
	},// closureDelete

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to add buttons */
	closureAdd: function(/*JBST*/ template, /*Artist*/ artist, /*bool*/ prepend) {

		// this will be the add button onclick event
		return function() {
			// look for sibling item to repair zebra striping
			var elem = $(this).parents(".panel").find(".view-item");

			if (!elem.length) {
				// no rows, so use column header
				prepend = false;
				elem = $(this).parents(".panel").find(".grid-header");
			}

			if (prepend) {
				// trim to first
				elem = elem.eq(0);
			} else {
				// trim to last
				elem = elem.eq(elem.length-1);
			}

			var member = {
				"ArtistID": artist.ArtistID
			};
			var item = template.bind(
				member,
				elem.is(".view-item-alt") ? 1 : 0,
				artist.Members.length+1);

			if (prepend) {
				// insert at top
				elem.beforeFade(item);
			} else {
				// insert at bottom
				elem.afterFade(item);
			}

			return false;
		};
	}// closureAdd
};