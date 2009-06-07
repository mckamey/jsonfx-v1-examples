using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Globalization;

namespace CalendarApp.WikiDump.MediaWiki
{
	public class WikiBot
	{
		#region Constants

		private const string WikiUrl = "http://en.wikipedia.org/w/api.php?action=query&format=xml&prop=revisions&rvprop=content&titles={0}";

		#endregion Constants

		#region Methods

		public IEnumerable<EventDto> FindEvents()
		{
			const int QueryLeapYear = 2000;
			DateTime date = new DateTime(QueryLeapYear, 1, 1);

			WikiParser parser = new WikiParser();
			List<EventDto> results = new List<EventDto>(366*100);

			// cycle through each day of a leap year
			while (date.Year == QueryLeapYear)
			{
				string response = new WebClient().DownloadString(String.Format(
					WikiUrl,
					date.ToString("MMMM'_'d", DateTimeFormatInfo.InvariantInfo)));

				results.AddRange(parser.Parse(date.Month, date.Day, response));

				// poor-man's throttling
				Thread.Sleep(500);
				date = date.AddDays(1);
			}

			return results;
		}

		#endregion Methods
	}
}
