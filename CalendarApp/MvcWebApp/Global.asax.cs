using System;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

using CalendarApp.Models;

namespace CalendarApp
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private void EnsureDatabase()
		{
			string connection = ConfigurationManager.ConnectionStrings["CalendarConnectionString"].ConnectionString;
			if (connection != null && connection.IndexOf("|DataDirectory|") >= 0)
			{
				string dataDir = HostingEnvironment.MapPath("~/App_Data");
				if (!Directory.Exists(dataDir))
				{
					Directory.CreateDirectory(dataDir);
				}
				connection = connection.Replace("|DataDirectory|", dataDir);
			}

			CalendarDataContext db = new CalendarDataContext(connection);
			if (db.DatabaseExists())
			{
				return;
			}

			// create the DB
			db.CreateDatabase();

			// add a system user
			Person sysadmin = new Person();
			sysadmin.OpenID = "http://example.com/openid";
			sysadmin.FirstName = "System";
			sysadmin.LastName = "Admin";
			sysadmin.CreatedDate = DateTime.UtcNow;
			sysadmin.CreatedBy = 1L;// created self
			db.Persons.InsertOnSubmit(sysadmin);

			db.SubmitChanges();
		}

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
					action = "Week",
					year = -1,
					month = -1,
					day = -1
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
			EnsureDatabase();
		}
	}
}