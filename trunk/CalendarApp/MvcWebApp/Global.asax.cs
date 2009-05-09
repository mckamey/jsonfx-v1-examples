using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;

using MvcWebApp.Models;

namespace MvcWebApp
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			//http://msdn.microsoft.com/en-us/magazine/2009.01.extremeaspnet.aspx
			//http://stephenwalther.com/blog/archive/2008/08/07/asp-net-mvc-tip-30-create-custom-route-constraints.aspx

			routes.MapRoute(
				"Calendar.Root",
				"",
				new
				{
					controller = "Calendar",
					action = "Year",
					year = -1
				}
			);

			routes.MapRoute(
				"Calendar.Year",
				"{year}",
				new
				{
					controller = "Calendar",
					action = "Year",
					year = -1
				},
				new
				{
					year = @"\d{1,4}"
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
					month = -1
				},
				new
				{
					year = @"\d{1,4}",
					month = @"\d{1,2}"
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
				},
				new
				{
					year = @"\d{1,4}",
					month = @"\d{1,2}",
					day = @"\d{1,2}"
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
			TimeUtility.SetupTimeZone(this.Context);
		}
	}
}