using System;
using System.Collections.Generic;

using CalendarApp.WikiDump.MediaWiki;

namespace CalendarApp.WikiDump
{
	class Program
	{
		static void Main(string[] args)
		{
			IEnumerable<EventDto> results = new WikiBot().FindEvents();
		}
	}
}
