using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Globalization;

namespace CalendarApp.WikiDump.MediaWiki
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// http://en.wikipedia.org/w/api.php?action=help
	/// </remarks>
	public class WikiBot
	{
		#region Constants

		private const string WikiUrl = "http://en.wikipedia.org/w/api.php?action=query&format=xml&prop=revisions&rvprop=content&titles={0}";

		#endregion Constants

		#region Methods

		public IEnumerable<IEnumerable<EventDto>> FindEvents()
		{
			const int QueryLeapYear = 2000;
			DateTime date = new DateTime(QueryLeapYear, 1, 1);

			WikiParser parser = new WikiParser();

			// cycle through each day of a leap year
			while (date.Year == QueryLeapYear)
			{
				// poor-man's throttling so as to not bombard wikipedia
				Console.WriteLine("Throttling...");
				Thread.Sleep(1000);

				string response = new WebClient().DownloadString(String.Format(
					WikiUrl,
					date.ToString("MMMM'_'d", DateTimeFormatInfo.InvariantInfo)));

				var events = parser.Parse(date.Month, date.Day, response);

				Console.WriteLine("Extracted {0} events for {1:MMM-dd}.", events.Count(), date);

				yield return events;

				date = date.AddDays(1);
			}
		}

		#endregion Methods
	}
}
