using System;

namespace DemoApp.Layouts
{
	public partial class Layout : System.Web.UI.MasterPage
	{
		protected override void OnInit(EventArgs e)
		{
			this.ID = "M";

			base.OnInit(e);
		}
	}
}
