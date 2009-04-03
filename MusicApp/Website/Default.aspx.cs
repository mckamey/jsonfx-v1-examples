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

			this.Grid.InlineData = service.GetArtist(5L);
			this.List.InlineData = service.GetArtist(1L);
			this.Edit.InlineData = service.GetMember(15L);
			this.Grid.DataItems["Music.WikipediaRoot"] = "http://en.wikipedia.org/wiki/";
		}
	}
}
