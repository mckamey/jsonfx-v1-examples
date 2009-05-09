using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;

namespace MvcWebApp
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			//http://msdn.microsoft.com/en-us/magazine/2009.01.extremeaspnet.aspx

			routes.MapRoute(
				"Calendar.Year",
				"{year}",
				new
				{
					controller = "Calendar",
					action = "Year",
					year = -1,
					month = -1,
					day = -1
				}
			);

			routes.MapRoute(
				"Calendar.Month",
				"{year}/{month}",
				new
				{
					controller = "Calendar",
					action = "Month",
					year = -1,
					month = -1,
					day = -1
				}
			);

			routes.MapRoute(
				"Calendar.Day",
				"{year}/{month}/{day}",
				new
				{
					controller = "Calendar",
					action = "Day",
					year = -1,
					month = -1,
					day = -1
				}
			);
		}

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
		}

		protected void Application_BeginRequest()
		{
			// establish the user's timezone for this request

			HttpCookie cookie = this.Context.Request.Cookies.Get("tz");
			TimeZoneInfo timezone;

			int offset;
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

			this.Context.Items["TimeZone"] = timezone;
		}
	}
}