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

			MusicApp.Services.MusicService service = new MusicApp.Services.MusicService();

			this.Grid.InlineData = service.GetMembers(5L);
			this.List.InlineData = service.GetMembers(1L);
			this.All.InlineData = service.GetArtists();
		}
	}
}
