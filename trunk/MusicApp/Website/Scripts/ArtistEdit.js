/*global Music, $ */

/*
	this is core logic for edit events and actions
*/

/* Music namespace */
if ("undefined" === typeof window.Music) {
	window.Music = {};
}

/* singleton */
Music.ArtistEdit = {

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to view buttons */
	closureView: function(/*JBST*/ template, /*ArtistDto*/ artist) {

		// this will be the view button onclick handler
		return function() {

			var view = template.bind(artist);
			$(this).parents(".panel").replaceWithFade(view);

			return false;
		};
	},// closureView

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to load buttons */
	closureLoad: function(/*JBST*/ template, /*object*/ artistID) {

		// this will be the load button onclick handler
		return function() {

			var panel = $(this).parents(".panel");
			Music.Service.getMembers(
				artistID,
				{
					onSuccess: function(artist) {
						var view = template.bind(artist);
						panel.replaceWithFade(view);
					}
				});

			return false;
		};
	},// closureLoad

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to edit buttons */
	closureEdit: function(/*JBST*/ template, /*Artist*/ artist, /*int*/ index, /*int*/ count) {

		// this will be the edit button onclick handler
		return function() {
			var elem = template.bind(artist, index, count);
			$(this).parents(".view-item").replaceWith(elem);

			return false;
		};
	},// closureEdit

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to save buttons */
	closureSave: function(/*JBST*/ template, /*Artist*/ artist, /*int*/ index, /*int*/ count) {

		// this will be the save button onclick handler
		return function() {
			var form = $(this.form);

			// apply fields to the data item
			form.find(":text").each(function() {
					artist[this.name] = $(this).val();

					if (this.name == "ArtistID") {
						artist[this.name] = artist[this.name] ?
							Number(artist[this.name]) :
							null;
					}
				});

			// genre was stored on the table, grab a reference
			var genre = $(this).parents(".view")[0].genre;
			var isNew = !artist.ArtistID;

			var old = $(this).parents(".view-item");
			Music.Service.saveArtist(
				artist,
				{
					// this is the callback from the JSON-RPC service
					onSuccess: function(artist) {
						if (isNew) {
							// add the saved artist to the genre data
							// so view changes and sorts reflect the addition
							genre.Artists.unshift(artist);
						}

						// rebind artist and replace form
						var elem = template.bind(artist, index, count);
						old.replaceWith(elem);
					}
				});

			return false;
		};
	},// closureSave

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to cancel buttons */
	closureCancel: function(/*JBST*/ template, /*Artist*/ artist, /*int*/ index, /*int*/ count) {

		// this will be the cancel button onclick handler
		return function() {
			if (!artist.ArtistID) {
				// user was adding a new artist
				// so just remove form
				$(this).parents(".view-item").removeFade();
				return false;
			}

			// user was editing an existing artist
			// so rebind original data and replace form
			var elem = template.bind(artist, index, count);
			$(this).parents(".view-item").replaceWith(elem);

			return false;
		};
	},// closureCancel

	/*	generates a closure which maintains a reference to
		the originally bound data and the target template
		for attaching to delete buttons */
	closureDelete: function(/*JBST*/ template, /*Artist*/ artist) {

		// this will be the delete button onclick handler
		return function() {
			var button = $(this);

			Music.Confirm.show(
				/* message */
				"Are you sure you want to delete \""+artist.ArtistName+"\"?",

				/* OK action */
				function() {
					// genre was stored on the table, grab a reference
					var genre = button.parents(".view")[0].genre;

					var editForm = button.parents(".view-item");
					Music.Service.deleteArtist(
						artist.ArtistID,
						{
							// this is the callback from the JSON-RPC service
							onSuccess: function(/*bool*/ result) {
								var fixZebra = false;
							
								// add the saved artist to the genre data
								// so view changes and sorts reflect the addition
								for (var i=0; i<genre.Artists.length; i++) {
									if (genre.Artists[i].ArtistID === artist.ArtistID) {
										// remove the single artist
										genre.Artists.splice(i, 1);
										if (i !== 0 && i !== genre.Artists.length) {
											fixZebra = true;
										}
									}
								}

								// remove edit form
								if (fixZebra) {
									var grid = editForm.parents(".panel");
									editForm.removeFade(300, function() {
										// fix zebra-stripes by rebinding entire grid
										var view = template.bind(genre);
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
	closureAdd: function(/*JBST*/ template, /*Genre*/ genre, /*bool*/ prepend) {

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

			var artist = {};
			var item = template.bind(
				artist,
				elem.is(".view-item-alt") ? 1 : 0,
				genre.Artists.length+1);

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