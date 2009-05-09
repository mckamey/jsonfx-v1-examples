using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace MvcWebApp.Controllers
{
    public class CalendarController : Controller
	{
		#region Controller Actions

		public ActionResult Year(int year, int month, int day)
        {
			this.ViewData["UserDate"] = this.BuildDate(year, month, day);

			return View();
        }

		public ActionResult Month(int year, int month, int day)
		{
			this.ViewData["UserDate"] = this.BuildDate(year, month, day);

			return View();
		}

		public ActionResult Day(int year, int month, int day)
		{
			this.ViewData["UserDate"] = this.BuildDate(year, month, day);

			return View();
		}

		#endregion Controller Actions

		#region Utility Methods

		private DateTime BuildDate(int year, int month, int day)
		{
			DateTime now = DateTime.UtcNow;

			if (year <= 0 || year >= 9999)
			{
				return new DateTime(now.Year, now.Month, now.Day, 12, 0, 0, DateTimeKind.Unspecified);
			}

			if (month < 1 || month > 12)
			{
				return new DateTime(year, now.Month, now.Day, 12, 0, 0, DateTimeKind.Unspecified);
			}

			if (day < 1 || day > 31)
			{
				return new DateTime(year, month, 1, 12, 0, 0, DateTimeKind.Unspecified);
			}

			return new DateTime(year, month, day, 12, 0, 0, DateTimeKind.Unspecified);
		}

		#endregion Utility Methods
	}
}
