using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

public interface ISlideView
{
	int SlideViewIndex { get; set; }
}

/// <summary>
/// Loads the view which corresponds to the URL.
/// </summary>
public class SimpleFrontController : IHttpHandlerFactory
{
	#region Constants

	private const string DefaultPath = "~/Default.aspx";
	public static readonly SlideDefn[] Slides = new SlideDefn[]
	{
		new SlideDefn("Intro", "Hello World!", "Example.introSlide"),
		new SlideDefn("JBST", "JBST: JsonML+Browser-Side Templating", "Example.jbstSlide"),
		new SlideDefn("Services", "Ajax Services", "Example.servicesSlide"),
		new SlideDefn("Performance", "Performance Optimizations", "Example.mergeSlide"),
		new SlideDefn("Behaviors", "Dynamic Behavior Binding", "Example.behaviorSlide"),
		new SlideDefn("CssUserAgent", "UserAgent-Specific CSS", "Example.userAgentSlide"),
		new SlideDefn("i18n/L10n", "Client-Side Globalization", "Example.globalizationSlide"),
		new SlideDefn("History", "Ajax History Support", "Example.historySlide"),
		new SlideDefn("Source Code", "Browse The Source Code", "Example.downloadSlide")
	};

	#endregion Constants

	#region Methods

	private int PickSlide(string slide)
	{
		int index;
		if (Int32.TryParse(slide, out index))
		{
			return index;
		}

		slide = SlideDefn.NormalizeUrl(slide);
		for (int i=0; i<SimpleFrontController.Slides.Length; i++)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(SimpleFrontController.Slides[i].Url, slide))
			{
				return i;
			}
		}
		return 0;
	}

	#endregion Methods

	#region IHttpHandlerFactory Members

	IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string virtualPath, string physicalPath)
	{
		Page page = BuildManager.CreateInstanceFromVirtualPath(DefaultPath, typeof(Page)) as Page;
		if (page is ISlideView)
		{
			if (virtualPath == null || virtualPath.Length < 1)
			{
				virtualPath = "/";
			}

			// pick the slide based upon the URL
			((ISlideView)page).SlideViewIndex = this.PickSlide(virtualPath.Substring(1));
		}
		return page;
	}

	void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
	{
	}

	#endregion IHttpHandlerFactory Members
}
