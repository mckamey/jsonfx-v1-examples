using System;

namespace CalendarApp.WikiDump.MediaWiki
{
	/// <summary>
	/// Note this DTO semantically matches the CalendarApp.Models.Event class.
	/// </summary>
	public class EventDto
	{
		public DateTime Starting { get; set; }

		public DateTime Ending { get; set; }

		public bool DateOnly { get; set; }

		public string Label { get; set; }

		public string Details { get; set; }
	}
}
