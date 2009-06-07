using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CalendarApp.WikiDump.MediaWiki
{
	public class WikiParser
	{
		#region Constants

		private const string StartToken = "==Events==";
		private const string EndToken = "==";
		private static readonly char[] LineDelim = { '\n', '\r' };
		private const string Pattern_WikiEvent = @"^[*]\s*(\d{1,4})\s*[&]ndash;\s*(.*)$";
		private static readonly Regex Regex_WikiEvent = new Regex(Pattern_WikiEvent,
			RegexOptions.Compiled|RegexOptions.ECMAScript|RegexOptions.IgnoreCase|RegexOptions.Multiline);

		#endregion Constants

		#region Methods

		public IEnumerable<EventDto> Parse(int month, int day, string response)
		{
			// hydrate the response as XML
			var doc = XDocument.Parse(response, LoadOptions.PreserveWhitespace);

			// extract WikiText from within the XML
			var text =
				(from node in doc.DescendantNodes().OfType<XElement>()
				 where node.Name == "rev" && !String.IsNullOrEmpty(node.Value)
				 select node.Value).FirstOrDefault();

			// parse the WikiText to produce events
			return this.ParseWikiText(month, day, text);
		}

		private IEnumerable<EventDto> ParseWikiText(int month, int day, string text)
		{
			if (String.IsNullOrEmpty(text))
			{
				yield break;
			}

			int start = text.IndexOf(StartToken);
			int end = text.IndexOf(EndToken, start+StartToken.Length);

			if (start < 0)
			{
				yield break;
			}
			start += StartToken.Length;

			if (end > start)
			{
				text = text.Substring(start, end-start);
			}
			else
			{
				text = text.Substring(start);
			}

			text = this.ParseWikiLinks(text);
			MatchCollection matches = Regex_WikiEvent.Matches(text);
			foreach (Match match in matches)
			{
				if (!match.Success)
				{
					continue;
				}

				int year;
				if (!Int32.TryParse(match.Groups[1].Value, out year) ||
					year < 1753) // SQL date min value
				{
					continue;
				}

				string label = match.Groups[2].Value;

				DateTime date = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
				yield return new EventDto
				{
					Starting = date,
					Ending = date.AddDays(1).AddSeconds(-1),
					DateOnly = true,
					Label = label,
					Details = label+String.Format(@"<br/><br/>Source <a href=""http://en.wikipedia.org/wiki/{0:MMMM'_'d}"">Wikipedia.org</a>", date)
				};
			}
		}

		private string ParseWikiLinks(string text)
		{
			StringBuilder builder = new StringBuilder(text.Length);

			int start = 0;
			for (int index=0; index<text.Length; index++)
			{
				if (text[index] == '[' &&
					index+1 < text.Length &&
					text[index+1] == '[')
				{
					builder.Append(text, start, index-start);
					start = index;
					this.ParseWikiLink(text, ref index, builder);
					start = index;
				}
			}

			builder.Append(text, start, text.Length-start);
			return builder.ToString();
		}

		private void ParseWikiLink(string line, ref int index, StringBuilder builder)
		{
			// consume link closing
			index += 2;

			for (int start=index; index<line.Length; index++)
			{
				if (line[index] == '|')
				{
					start = index+1;
					continue;
				}

				if (line[index] == ']')
				{
					builder.Append(line, start, index-start);
					break;
				}
			}

			while (index+1 < line.Length &&
				line[index] != ']' &&
				line[index+1] != ']')
			{
				index++;
			}

			// consume link closing
			index += 2;
		}

		#endregion Methods
	}
}
