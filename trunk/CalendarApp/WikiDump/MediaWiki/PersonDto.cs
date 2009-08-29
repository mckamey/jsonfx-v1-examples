using System;

namespace CalendarApp.WikiDump.MediaWiki
{
	/// <summary>
	/// Note this DTO semantically matches fields of CalendarApp.Models.Person class.
	/// </summary>
	public class PersonDto
	{
		public long PersonID { get; set; }

		public string OpenID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}
