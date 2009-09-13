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

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!this.Context.IsDebuggingEnabled)
			{
				// improve the Yslow rating
				JsonFx.Handlers.ResourceHandler.EnableStreamCompression(this.Context);
			}
		}

		protected override void OnError(EventArgs e)
		{
			if (!this.Context.IsDebuggingEnabled)
			{
				// improve the Yslow rating
				JsonFx.Handlers.ResourceHandler.DisableStreamCompression(this.Context);
			}

			base.OnError(e);
		}
	}
}
