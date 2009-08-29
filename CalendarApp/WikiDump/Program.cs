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
				Console.WriteLine(@"Enter the website root (e.g. ""http://localhost:49331/"") to begin extracting events from Wikipedia.org:");
				string host = Console.ReadLine();
				Uri uri;
				if (!Uri.TryCreate(host, UriKind.Absolute, out uri))
				{
					Console.Error.WriteLine("That doesn't look like a valid URL.");
					return;
				}
				Console.WriteLine("Extracting events from Wikipedia.org...");

				PersonDto user = JsonRpcUtility.CallService<PersonDto>(
					new Uri(uri, "/Services/CalendarService.jrpc"),
					"savePerson",
					new PersonDto { OpenID="http://en.wikipedia.org/w/api.php?action=help", FirstName="Wiki", LastName="Pedia" });

				foreach (var results in new WikiBot().FindEvents())
				{
					Console.WriteLine("Saving {0} events to {1}", results.Count(), uri.Host);

					JsonRpcUtility.CallService<List<EventDto>>(
						new Uri(uri, "/Services/CalendarService.jrpc"),
						"saveEvents",
						results,
						user.PersonID);

					Console.WriteLine("Success.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				Console.WriteLine("Press any key to end...");
				Console.ReadKey();
			}
		}
	}
}
