using System;
using System.Web;

namespace MvcWebApp.Models
{
	public class TimeUtility
	{
		#region Methods

		public static DateTime BuildDate(int year, int month, int day)
		{
			if (year <= 0 || year >= 9999)
			{
				return DateTime.UtcNow;
			}

			if (month < 1 || month > 12)
			{
				// unspecified is browser time zone
				return new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
			}

			if (day < 1 || day > 31)
			{
				// unspecified is browser time zone
				return new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Unspecified);
			}

			// unspecified is browser time zone
			return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
		}

		/// <summary>
		/// Establishes the user's timezone for this request
		/// </summary>
		/// <param name="context"></param>
		public static void SetupTimeZone(HttpContext context)
		{
			HttpCookie cookie = context.Request.Cookies.Get("tz");

			int offset;
			TimeZoneInfo timezone;
			if (cookie == null || !Int32.TryParse(cookie.Value, out offset))
			{
				// use server time
				timezone = TimeZoneInfo.Local;
			}
			else
			{
				// use browser time
				timezone = TimeZoneInfo.CreateCustomTimeZone(
					cookie.Value,
					TimeSpan.FromMinutes(-offset),// reverse direction
					cookie.Value,
					cookie.Value);
			}

			context.Items["TimeZone"] = timezone;
		}

		#endregion Methods
	}
}
