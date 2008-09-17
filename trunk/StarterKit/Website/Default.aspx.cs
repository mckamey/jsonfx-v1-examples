using System;

public partial class _Default : System.Web.UI.Page
{
	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

#if DEBUG
		// Pretty-Print rendering helps when debugging through script
		this.StyleImport.IsDebug = true;
		this.ScriptInclude.IsDebug = true;
#endif
	}
}
