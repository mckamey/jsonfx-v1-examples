/*global JsonFx, jQuery, Music */

/* CssUserAgent --------------------------------------------------------- */

/* enable valid CSS to target browsers without reverting to CSS hacks */
JsonFx.UA.setCssUserAgent();

/* loading indicator --------------------------------------------------------- */

/* setup Ajax loading indicator */
JsonFx.IO.Service.prototype.onBeginRequest = function() {
	// only show if request takes longer than 200ms
	Music.Loading.show(200);
};

JsonFx.IO.Service.prototype.onEndRequest = function() {
	Music.Loading.hide();
};

/* behavior bindings --------------------------------------------------------- */

JsonFx.Bindings.add(
	"a.ext-link",
	function(elem) {
		$(elem).click(function() {
			window.open(this.href);
			return false;
		});
	});

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
	elem = $(elem).fadeTo(0, 0.0);
	this.fadeOut(fade).replaceWith(
		elem.fadeTo(fade, 1.0)
	);
};

jQuery.fn.removeFade = function(/*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	this.fadeOut(fade, function() {
		$(this).remove();
	});
};

jQuery.fn.beforeFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	$(this).before($(elem).fadeIn(fade));
};

jQuery.fn.prependFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	$(this).prepend($(elem).fadeIn(fade));
};

jQuery.fn.appendFade = function(/*DOM*/ elem, /*string|number*/ fade) {
	if ("undefined" === typeof fade) {
		fade = 300;
	}

	$(this).append($(elem).fadeIn(fade));
};
