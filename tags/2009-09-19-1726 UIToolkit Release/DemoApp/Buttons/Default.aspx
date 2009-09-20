<%@ Page Language="C#" MasterPageFile="~/Layouts/Layout.Master" Title="DemoApp - Dialogs" %>

<asp:Content runat="server" ContentPlaceHolderID="B">

	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
	<script type="text/javascript">
	
		function chooseButton(parent) {
			$(this)
				.parents(parent)
				.find(".button-active")
				.removeClass('button-active')
				.html('Normal')
				.attr("href", "#normal");

			$(this)
				.addClass('button-active')
				.html('Active')
				.attr("href", "#active");;

			return false;
		}

	</script>

	<h2>Button States</h2>

	<div class="buttons">
		<p style="clear:left;margin:2em 0">
			<a href="#disabled" class="button button-disabled"
				onclick="return false;">Disabled</a>
		</p>
		<p style="clear:left;margin:2em 0">
			<a href="#active" class="button button-active"
				onclick="return chooseButton.call(this, '.buttons');">Active</a>
		</p>
		<p style="clear:left;margin:2em 0">
			<a href="#normal" class="button"
				onclick="return chooseButton.call(this, '.buttons');">Normal</a>
		</p>
		<p style="clear:left;margin:2em 0">
			<a href="#normal" class="button"
				onclick="return chooseButton.call(this, '.buttons');">Normal</a>
		</p>
		<p style="clear:left;margin:2em 0">
			<a href="#reverse" class="button button-reverse" onclick="return false;">Reverse</a>
		</p>
	</div>

	<h2>Button Toolbar</h2>

	<ul class="button-toolbar" style="clear:left;margin:2em 0">
		<li>
			<a href="#active" class="button button-active"
				onclick="return chooseButton.call(this, '.button-toolbar');">Active</a>
		</li>
		<li>
			<a href="#normal" class="button"
				onclick="return chooseButton.call(this, '.button-toolbar');">Normal</a>
		</li>
		<li>
			<a href="#normal" class="button"
				onclick="return chooseButton.call(this, '.button-toolbar');">Normal</a>
		</li>
		<li>
			<a href="#disabled" class="button button-disabled"
				onclick="return false;">Disabled</a>
		</li>
		<li>
			<a href="#normal" class="button"
				onclick="return chooseButton.call(this, '.button-toolbar');">Normal</a>
		</li>
		<li>
			<a href="#reverse" class="button button-reverse"
				onclick="return false;">Reverse</a>
		</li>
	</ul>

</asp:Content>
