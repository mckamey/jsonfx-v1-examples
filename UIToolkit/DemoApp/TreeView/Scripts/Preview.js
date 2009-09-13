/* namespace DemoApp */
var DemoApp;
if ("undefined" === typeof DemoApp) {
	DemoApp = {};
}

/*-------------------------------------------------------------------*/

/*void*/ DemoApp.previewFile = function(/*string*/ data) {
	if (!DemoApp.previewFile) {
		return;
	}

	var preview = DemoApp.previewFile.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};

/*void*/ DemoApp.previewImage = function(/*string*/ data) {
	if (!DemoApp.previewImage) {
		return;
	}

	var preview = DemoApp.previewImage.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};
