using System;

namespace JsonFx.WapTemplate
{
	public partial class _Default : System.Web.UI.Page
	{
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
