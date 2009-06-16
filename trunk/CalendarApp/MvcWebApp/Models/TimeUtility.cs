using System;
using System.Web;

namespace CalendarApp.Models
{
	public class TimeUtility
	{
		#region Properties

		public static TimeZoneInfo BrowserTimeZone
		{
			get
			{
				HttpContext context = HttpContext.Current;
				TimeZoneInfo tz = context.Items["BrowserTimeZone"] as TimeZoneInfo;
				if (tz == null)
				{
					return TimeUtility.BuildBrowserTimeZone(context);
				}
				return tz;
			}
		}

		#endregion Properties

		#region Utility Methods

		/// <summary>
		/// Converts a date to the UTC time zone.
		/// </summary>
		/// <param name="date"></param>
		/// 
		/// <returns></returns>
		public static DateTime ToUtcTimeZone(DateTime date)
		{
			switch (date.Kind)
			{
				case DateTimeKind.Local:
				{
					// treat as server timezone
					return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, TimeZoneInfo.Utc);
				}
				case DateTimeKind.Unspecified:
				{
					// treat as browser timezone
					return TimeZoneInfo.ConvertTime(date, TimeUtility.BrowserTimeZone, TimeZoneInfo.Utc);
				}
				case DateTimeKind.Utc:
				default:
				{
					// treat as UTC
					return date;
				}
			}
		}

		/// <summary>
		/// Converts a date to the browser's time zone.
		/// </summary>
		/// <param name="date"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static DateTime ToBrowserTimeZone(DateTime date)
		{
			switch (date.Kind)
			{
				case DateTimeKind.Local:
				{
					// treat as server timezone
					return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, TimeUtility.BrowserTimeZone);
				}
				case DateTimeKind.Utc:
				{
					// treat as UTC
					return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Utc, TimeUtility.BrowserTimeZone);
				}
				default:
				case DateTimeKind.Unspecified:
				{
					// treat as browser timezone
					return date;
				}
			}
		}

		/// <summary>
		/// Builds an appropriate (browser-local) DateTime for the given range of inputs.
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <returns></returns>
		public static DateTime BuildDate(int year, int month, int day)
		{
			return BuildDate(year, month, day, 0, 0, 0);
		}

		/// <summary>
		/// Builds an appropriate (browser-local) DateTime for the given range of inputs.
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <returns></returns>
		public static DateTime BuildDate(int year, int month, int day, int hour, int minute, int second)
		{
			if (year < 1753 || year > 9999)
			{
				DateTime now = TimeUtility.ToBrowserTimeZone(DateTime.UtcNow);

				// unspecified means browser time zone
				return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Unspecified);
			}

			if (month < 1 || month > 12)
			{
				// unspecified means browser time zone
				return new DateTime(year, 1, 1, hour, minute, second, DateTimeKind.Unspecified);
			}

			if (day < 1 || day > 31)
			{
				// unspecified means browser time zone
				return new DateTime(year, month, 1, hour, minute, second, DateTimeKind.Unspecified);
			}

			// unspecified means browser time zone
			return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
		}

		#endregion Utility Methods

		#region Private Methods

		/// <summary>
		/// Establishes the user's timezone for this request
		/// </summary>
		/// <param name="context"></param>
		private static TimeZoneInfo BuildBrowserTimeZone(HttpContext context)
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

			context.Items["BrowserTimeZone"] = timezone;

			return timezone;
		}

		#endregion Private Methods
	}
}
