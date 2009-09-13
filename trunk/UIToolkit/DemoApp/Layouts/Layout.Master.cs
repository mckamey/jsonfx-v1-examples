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

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Context.IsDebuggingEnabled)
			{
				// improve the Yslow rating
				JsonFx.Handlers.ResourceHandler.EnableStreamCompression(this.Context);
			}
		}
	}
}
