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
	private static readonly string NormalizedDefaultPath = SlideDefn.NormalizeUrl(DefaultPath.TrimStart('~', '/'));

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
		slide = SlideDefn.NormalizeUrl(slide);
		if (String.IsNullOrEmpty(slide))
		{
			return 0;
		}

		int index;
		if (Int32.TryParse(slide, out index))
		{
			return index;
		}

		for (int i=0; i<SimpleFrontController.Slides.Length; i++)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(SimpleFrontController.Slides[i].Url, slide))
			{
				return i;
			}
		}

		if (StringComparer.OrdinalIgnoreCase.Equals(SimpleFrontController.NormalizedDefaultPath, slide))
		{
			return 0;
		}

		return -1;
	}

	private string GetAppRoot()
	{
		string root = HttpRuntime.AppDomainAppVirtualPath;
		if (!root.EndsWith("/"))
		{
			root += '/';
		}
		return root;
	}

	#endregion Methods

	#region IHttpHandlerFactory Members

	IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string virtualPath, string physicalPath)
	{
		Page page = BuildManager.CreateInstanceFromVirtualPath(DefaultPath, typeof(Page)) as Page;
		if (page is ISlideView)
		{
			string slide = context.Request.AppRelativeCurrentExecutionFilePath;

			// trim leading "~/"
			slide = slide.TrimStart('~', '/');

			if (slide.IndexOf('/') > 0)
			{
				string root = this.GetAppRoot();
				slide = slide.Substring(0, slide.IndexOf('/'));
				context.Response.Redirect(root+slide);
				return null;
			}

			// pick the slide based upon the path
			int slideIndex = ((ISlideView)page).SlideViewIndex = this.PickSlide(slide);
			if (slideIndex < 0)
			{
				context.Response.Redirect(this.GetAppRoot());
				return null;
			}
		}
		return page;
	}

	void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
	{
	}

	#endregion IHttpHandlerFactory Members
}
