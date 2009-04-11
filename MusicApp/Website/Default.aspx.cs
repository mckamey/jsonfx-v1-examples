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

			// start off with a Music.ArtistGrid view of all artists
			this.Start.InlineData = new MusicApp.Services.MusicService().GetArtists();
		}
	}
}
