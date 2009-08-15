using System;
using System.Text.RegularExpressions;

using JsonFx.Json;

/// <summary>
/// Defines metadata around a single slide.
/// </summary>
public class SlideDefn
{
	#region Constants

	private static readonly Regex Regex_InvalidChars = new Regex(
		@"\W|[_]",
		RegexOptions.Compiled|RegexOptions.ECMAScript);

	#endregion Constants

	#region Fields

	private string url;
	private string name;
	private string title;
	private EcmaScriptIdentifier view;

	#endregion Fields

	#region Init

	/// <summary>
	/// Ctor
	/// </summary>
	/// <param name="name"></param>
	/// <param name="title"></param>
	/// <param name="view"></param>
	public SlideDefn(string name, string title, EcmaScriptIdentifier view)
		: this(name, title, view, SlideDefn.NormalizeUrl(name))
	{
	}

	/// <summary>
	/// Ctor
	/// </summary>
	/// <param name="name"></param>
	/// <param name="title"></param>
	/// <param name="view"></param>
	public SlideDefn(string name, string title, EcmaScriptIdentifier view, string url)
	{
		this.name = name;
		this.title = title;
		this.view = view;
		this.url = url;
	}

	#endregion Init

	#region Properties

	/// <summary>
	/// Gets and sets the name of the slide.
	/// </summary>
	[JsonName("name")]
	public string Name
	{
		get { return this.name; }
		set { this.name = value; }
	}

	/// <summary>
	/// Gets the normalized name of the slide.
	/// </summary>
	[JsonName("url")]
	public string Url
	{
		get { return this.url; }
		set { this.url = value; }
	}

	/// <summary>
	/// Gets and sets the title of the slide.
	/// </summary>
	[JsonName("title")]
	public string Title
	{
		get { return this.title; }
		set { this.title = value; }
	}

	/// <summary>
	/// Gets and sets the JBST name of the slide.
	/// </summary>
	[JsonName("jbst")]
	public EcmaScriptIdentifier View
	{
		get { return this.view; }
		set { this.view = value; }
	}

	#endregion Properties

	#region Methods

	/// <summary>
	/// Creates a URL-friendly version of the name
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string NormalizeUrl(string value)
	{
		if (String.IsNullOrEmpty(value))
		{
			return String.Empty;
		}

		return Regex_InvalidChars.Replace(value, "-").ToLowerInvariant();
	}

	#endregion Methods
}
