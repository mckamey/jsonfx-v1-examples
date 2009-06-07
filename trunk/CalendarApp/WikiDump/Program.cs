using System;
using System.Collections.Generic;
using System.Linq;

using JsonFx.JsonRpc;
using CalendarApp.WikiDump.MediaWiki;

namespace CalendarApp.WikiDump
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("Extracting events from Wikipedia.org...");

				foreach (var results in new WikiBot().FindEvents())
				{
					Console.WriteLine("Saving {0} events to http://localhost:49331", results.Count());

					JsonRpcUtility.CallService<List<EventDto>>(
						new Uri("http://localhost:49331/Services/CalendarService.jrpc"),
						"saveEvents",
						results);

					Console.WriteLine("Success.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			Console.ReadLine();
		}
	}
}
