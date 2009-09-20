<%@ Page Language="C#" MasterPageFile="~/Layouts/Layout.Master" Title="DemoApp - Dialogs" %>

<asp:Content runat="server" ContentPlaceHolderID="B">

	<h2>Dialogs</h2>

	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
	<script type="text/javascript">

		/* namespace DemoApp.Dialogs */
		var DemoApp;
		if ("undefined" === typeof DemoApp) {
			DemoApp = {};
		}
		if ("undefined" === typeof DemoApp.Dialogs) {
			DemoApp.Dialogs = {};
		}

		DemoApp.Dialogs.showMessage = function(/*string*/ message) {
			document.body.appendChild(document.createElement("br"));
			document.body.appendChild(document.createTextNode(message));
		}

		DemoApp.Dialogs.alertOk = function() {
			DemoApp.Dialogs.showMessage("'OK' clicked in alert.");
		}

		DemoApp.Dialogs.confirmYes = function() {
			DemoApp.Dialogs.showMessage("'Yes' clicked in confirmation dialog.");
		}

		DemoApp.Dialogs.confirmNo = function() {
			DemoApp.Dialogs.showMessage("'No' clicked in confirmation dialog.");
		}

		DemoApp.Dialogs.customSave = function() {
			DemoApp.Dialogs.showMessage("'Save' clicked in custom dialog.");
		}

		DemoApp.Dialogs.customCancel = function() {
			DemoApp.Dialogs.showMessage("'Cancel' clicked in custom dialog.");
		}
	
		DemoApp.Dialogs.customDelete = function() {
			DemoApp.Dialogs.showMessage("'Delete' clicked in custom dialog.");
		}
	
	</script>

	<ul class="bulleted">
		<li>
			<a href="#custom"
				onclick="DemoApp.Dialogs.CustomDialog.show(DemoApp.Dialogs.customSave, DemoApp.Dialogs.customCancel, DemoApp.Dialogs.customDelete);return false;">Custom Dialogs</a>
		</li>
		<li>
			<a href="#confirm"
				onclick="UIT.Confirm.show('This is a confirmation dialog.', DemoApp.Dialogs.confirmYes, DemoApp.Dialogs.confirmNo, 'Yes', 'No');return false;">Confirm Dialog</a>
		</li>
		<li>
			<a href="#alert"
				onclick="UIT.Alert.show('The alert title', 'This is an alert dialog.', DemoApp.Dialogs.alertOk);return false;">Alert Dialog (with title)</a>
		</li>
		<li>
			<a href="#alert"
				onclick="UIT.Alert.show(null, 'This is an alert dialog without a title.', DemoApp.Dialogs.alertOk);return false;">Alert Dialog (without title)</a>
		</li>
	</ul>

	<h2>Loading Indicator</h2>
	<ul class="bulleted">
		<li>
			<a href="#confirm"
				onclick="UIT.Loading.show();DemoApp.Dialogs.showMessage('Loading indicator count incremented.');return false;">Show Loading&hellip; (no delay)</a>
		</li>
		<li>
			<a href="#confirm"
				onclick="UIT.Loading.show(1000);DemoApp.Dialogs.showMessage('Loading indicator count incremented.');return false;">Show Loading&hellip; (1 sec delay)</a>
		</li>
		<li>
			<a href="#confirm"
				onclick="UIT.Loading.hide();DemoApp.Dialogs.showMessage('Loading indicator count decremented.');return false;">Hide Loading&hellip;</a>
		</li>
	</ul>

</asp:Content>
