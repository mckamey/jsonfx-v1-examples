/*global JsonFx, Music */

/* enable valid CSS to target browsers without reverting to CSS hacks */
JsonFx.UA.setCssUserAgent();

/* setup Ajax loading indicator */
JsonFx.IO.Service.prototype.onBeginRequest = function() {
	// only show if request takes longer than 200ms
	Music.Loading.show(200);
};

JsonFx.IO.Service.prototype.onEndRequest = function() {
	Music.Loading.hide();
};

JsonFx.Bindings.add(
	"a.ext-link",
	function(elem) {
		$(elem).click(function() {
			window.open(this.href);
			return false;
		});
	});