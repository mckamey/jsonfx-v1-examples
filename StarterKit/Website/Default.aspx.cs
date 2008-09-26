using System;
using System.Globalization;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		// switch the UI culture for the globalization example
		foreach (string lang in this.Context.Request.UserLanguages)
		{
			try
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
				break;
			}
			catch { }
		}

#if DEBUG
		// Pretty-Print rendering helps when debugging through script
		this.StyleImport.IsDebug = true;
		this.ScriptInclude.IsDebug = true;
#else
		// improve the Yslow rating
		JsonFx.Handlers.CompiledBuildResult.EnableStreamCompression(this.Context);
#endif
	}
}
