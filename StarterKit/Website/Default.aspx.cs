using System;
using System.Web;
using System.Globalization;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		SetupCulture(this.Context);

		this.PageData["App.JsonFxVersion"] = JsonFx.About.Fx.Version;
	}

	protected override void OnPreRenderComplete(EventArgs e)
	{
		base.OnPreRenderComplete(e);

		// improve the Yslow rating
		JsonFx.Handlers.ResourceHandler.EnableStreamCompression(this.Context);
	}

	protected override void OnError(EventArgs e)
	{
		// remove compression
		JsonFx.Handlers.ResourceHandler.DisableStreamCompression(this.Context);

		base.OnError(e);
	}

	/// <summary>
	/// Finds the best fit culture
	/// </summary>
	/// <param name="context"></param>
	public static void SetupCulture(HttpContext context)
	{
		if (context.Request.UserLanguages == null ||
			context.Request.UserLanguages.Length < 1)
		{
			// use default culture
			return;
		}

		Thread currentThread = Thread.CurrentThread;
		CultureInfo defaultCulture = currentThread.CurrentCulture;
		CultureInfo defaultUICulture = currentThread.CurrentUICulture;

		// cycle through each of the requested cultures
		// stop at the first which is supported
		foreach (string lang in context.Request.UserLanguages)
		{
			int index = (lang != null) ? lang.IndexOf(';') : -1;
			string culture = (index < 0) ? lang : lang.Substring(0, index);
			if (String.IsNullOrEmpty(culture))
			{
				continue;
			}

			try
			{
				currentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
				currentThread.CurrentUICulture = new CultureInfo(culture);

				// each culture has a value which says what it is
				// so we can tell if it is just defaulting to the
				// default culture
				if (culture.StartsWith(Resources.Example._CultureName, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
			}
			catch { }

			// reset since wasn't a supported culture
			currentThread.CurrentCulture = defaultCulture;
			currentThread.CurrentUICulture = defaultUICulture;
		}
	}
}
