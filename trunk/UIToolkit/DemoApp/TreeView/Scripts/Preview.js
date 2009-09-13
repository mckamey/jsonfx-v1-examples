/* namespace DemoApp */
var DemoApp;
if ("undefined" === typeof DemoApp) {
	DemoApp = {};
}

/*-------------------------------------------------------------------*/

/*void*/ DemoApp.previewFile = function(/*string*/ data) {
	if (!DemoApp.TreeView.PreviewFile) {
		return;
	}

	var preview = DemoApp.TreeView.PreviewFile.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};

/*void*/ DemoApp.previewImage = function(/*string*/ data) {
	if (!DemoApp.TreeView.PreviewImage) {
		return;
	}

	var preview = DemoApp.TreeView.PreviewImage.bind(data);

	if (preview) {
		document.body.insertBefore(preview, document.body.firstChild);
	}
};
