using System;

namespace MusicApp
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

			this.Grid.InlineData = new MusicApp.Services.MusicService().GetArtist(5L);
			this.List.InlineData = new MusicApp.Services.MusicService().GetArtist(1L);
		}
	}
}
