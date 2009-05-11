using System;

using JsonFx.BuildTools.HtmlDistiller;
using JsonFx.BuildTools.HtmlDistiller.Filters;
using JsonFx.BuildTools.HtmlDistiller.Writers;

namespace CalendarApp.Models
{
	/// <summary>
	/// This defines a fully configurable set of allowed markup.
	/// </summary>
	internal class RichTextFilter : SafeHtmlFilter
	{
		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		private RichTextFilter()
			: base(20, true)
		{
		}

		#endregion Init

		#region Utility Methods

		/// <summary>
		/// Cleanses the markup.
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string Scrub(string html)
		{
			return HtmlDistiller.Parse(html, new RichTextFilter());
		}

		#endregion Utility Methods

		#region IHtmlFilter Members

		/// <summary>
		/// Determines what tags are allowed.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public override bool FilterTag(HtmlTag tag)
		{
			// whitelist tags
			switch (tag.TagName.ToLowerInvariant())
			{
				case "a":
				case "b":
				case "em":
				case "hr":
				case "i":
				case "img":
				case "li":
				case "ol":
				case "p":
				case "strong":
				case "u":
				case "ul":
				{
					return base.FilterTag(tag);
				}
				default:
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Determines which attributes are allowed.
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="attribute"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool FilterAttribute(string tag, string attribute, ref string value)
		{
			// whitelist attributes per tag
			switch (tag.ToLowerInvariant())
			{
				case "a":
				{
					switch (attribute.ToLowerInvariant())
					{
						case "href":
						{
							return base.FilterAttribute(tag, attribute, ref value);
						}
						default:
						{
							return false;
						}
					}
				}
				case "img":
				{
					switch (attribute.ToLowerInvariant())
					{
						case "src":
						case "alt":
						{
							return base.FilterAttribute(tag, attribute, ref value);
						}
						default:
						{
							return false;
						}
					}
				}
			}

			return false;
		}

		public override bool FilterStyle(string tag, string style, ref string value)
		{
			return false;
		}

		#endregion IHtmlFilter Members
	}
}
