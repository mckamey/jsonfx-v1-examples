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

		#region Methods

		public override bool Equals(object that)
		{
			if (that is RichTextValue)
			{
				return this.Value.Equals(((RichTextValue)that).Value);
			}

			return this.Value.Equals(that);
		}

		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}

		public static implicit operator RichTextValue(string value)
		{
			return new RichTextValue(value);
		}

		public static explicit operator string(RichTextValue value)
		{
			return value.Value;
		}

		#endregion Methods
	}
}
