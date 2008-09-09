/*global JsonFx */
/*
	Example.js

	Example script for the Starter Kit
*/

/* enable valid CSS to target browsers without reverting to CSS hacks */
JsonFx.UI.setCssUserAgent();

/*
	it is a best practice to not clutter the global namespace
	creating top level objects which contain variables and functions
	allows us to simulate namespaces
*/

/* namespace Example */
if ("undefined" === typeof window.Example) {
	window.Example = {};
}
