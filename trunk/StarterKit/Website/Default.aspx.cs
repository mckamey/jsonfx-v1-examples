using System;

public partial class _Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
#if DEBUG
		this.StyleImport.IsDebug = true;
		this.ScriptInclude.IsDebug = true;
#endif
	}
}
