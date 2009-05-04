using System;

namespace MusicApp
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// start off with a Music.ArtistGrid view of all artists
			this.Start.InlineData = new MusicApp.Services.MusicService().GetArtists();
		}

		protected override void OnPreRenderComplete(EventArgs e)
		{
			base.OnPreRenderComplete(e);

			// improve the Yslow rating
			JsonFx.Handlers.ResourceHandler.EnableStreamCompression(this.Context);
		}

		protected override void OnError(EventArgs e)
		{
			try
			{
				// remove compression
				JsonFx.Handlers.ResourceHandler.DisableStreamCompression(this.Context);
			}
			catch {}

			base.OnError(e);
		}
	}
}
