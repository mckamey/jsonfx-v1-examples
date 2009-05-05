/*global JsonFx, jQuery, Music */

/* CssUserAgent --------------------------------------------------------- */

/* enable valid CSS to target browsers without reverting to CSS hacks */
JsonFx.UA.setCssUserAgent();

/* service callbacks --------------------------------------------------------- */

/* setup Ajax loading indicator */
JsonFx.IO.Service.prototype.onBeginRequest = function() {
	// only show if request takes longer than 200ms
	Music.Loading.show(200);
};

JsonFx.IO.Service.prototype.onEndRequest = function() {
	Music.Loading.hide();
};

/* assign a default Ajax error handler */
JsonFx.IO.onFailure = function(/*XMLHttpRequest*/ xhr, /*context*/ cx, /*Error*/ ex) {
	var msg = ex && ex.message || ex;
	Music.Alert.show("Sorry an error occurred...", msg, null);
};

/* jQuery extensions --------------------------------------------------------- */

if (!jQuery.support.opacity) {
	// IE8 and below don't fade very nicely so just make speedier by disabling animation
	jQuery.fx.off = true;
}

jQuery.fn.replaceWithFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	// show/hide affect CSS display, so using fadeTo
	elem = jQuery(elem).fadeTo(0, 0.0);
	this.fadeOut(fade).replaceWith(
		elem.fadeTo(fade, 1.0)
	);
};

jQuery.fn.removeFade = function(/*string|number*/ fade, /*function*/ cb) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	this.fadeOut(fade, function() {
		jQuery(this).remove();
		if ("function" === typeof cb) {
			cb.call(this);
		}
	});
};

jQuery.fn.beforeFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	jQuery(this).before(jQuery(elem).fadeIn(fade));
};

jQuery.fn.afterFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	jQuery(this).after(jQuery(elem).fadeIn(fade));
};

jQuery.fn.prependFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	jQuery(this).prepend(jQuery(elem).fadeIn(fade));
};

jQuery.fn.appendFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	jQuery(this).append(jQuery(elem).fadeIn(fade));
};
