using System;
using System.Globalization;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		this.SetupUICulture();

#if DEBUG
		// Pretty-Print rendering helps when debugging through script
		this.StyleImport.IsDebug = true;
		this.ScriptInclude.IsDebug = true;
#else
		// improve the Yslow rating
		JsonFx.Handlers.CompiledBuildResult.EnableStreamCompression(this.Context);
#endif
	}

	private void SetupUICulture()
	{
		// switch the UI culture for the globalization example
		foreach (string lang in this.Context.Request.UserLanguages)
		{
			if (String.IsNullOrEmpty(lang))
			{
				continue;
			}

			int index = lang.IndexOf(';');
			string culture = index < 0 ? lang : lang.Substring(0, index);
			try
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
				break;
			}
			catch { }
		}
	}
}
