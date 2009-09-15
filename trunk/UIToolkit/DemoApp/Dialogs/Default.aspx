<%@ Page Language="C#" MasterPageFile="~/Layouts/Layout.Master" Title="DemoApp - Dialogs" %>

<asp:Content runat="server" ContentPlaceHolderID="B">

	<h2>Dialogs</h2>

	<script type="text/javascript">

		function showMessage(/*string*/ message) {
			document.body.appendChild(document.createElement("br"));
			document.body.appendChild(document.createTextNode(message));
		}

		function alertOk() {
			showMessage("'OK' clicked in alert.");
		}

		function confirmNo() {
			showMessage("'No' clicked in confirm dialog.");
		}

		function confirmYes() {
			showMessage("'Yes' clicked in confirm dialog.");
		}
	
	</script>

	<ul class="bulleted">
		<li>
			<a href="#alert"
				onclick="UIT.Alert.show('The alert title', 'This is an alert dialog.', alertOk);return false;">Alert Dialog (with title)</a>
		</li>
		<li>
			<a href="#alert"
				onclick="UIT.Alert.show(null, 'This is an alert dialog without a title.', alertOk);return false;">Alert Dialog (without title)</a>
		</li>
		<li>
			<a href="#confirm"
				onclick="UIT.Confirm.show('This is a confirm dialog.', confirmYes, confirmNo, 'Yes', 'No');return false;">Confirm Dialog</a>
		</li>
	</ul>

	<h2>Loading Indicator</h2>
	<ul class="bulleted">
		<li>
			<a href="#confirm"
				onclick="UIT.Loading.show();showMessage('Loading indicator count incremented.');return false;">Show Loading&hellip; (no delay)</a>
		</li>
		<li>
			<a href="#confirm"
				onclick="UIT.Loading.show(1000);showMessage('Loading indicator count incremented.');return false;">Show Loading&hellip; (1 sec delay)</a>
		</li>
		<li>
			<a href="#confirm"
				onclick="UIT.Loading.hide();showMessage('Loading indicator count decremented.');return false;">Hide Loading&hellip;</a>
		</li>
	</ul>

</asp:Content>
