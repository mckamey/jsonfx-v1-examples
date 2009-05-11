using System;

namespace CalendarApp.Models
{
	/// <summary>
	/// A safe way to lazy scrub user content.
	/// </summary>
	public class RichTextValue
	{
		#region Fields

		private bool isSafe;
		private string value;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="value"></param>
		public RichTextValue(string value)
		{
			this.value = value ?? "";
		}

		#endregion Init

		#region Properties

		public string Value
		{
			get
			{
				if (!this.isSafe)
				{
					this.value = RichTextFilter.Scrub(this.value);
					this.isSafe = true;
				}
				return this.value;
			}
		}

		#endregion Properties
	}
}
