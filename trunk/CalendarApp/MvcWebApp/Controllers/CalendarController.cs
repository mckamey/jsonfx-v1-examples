using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using CalendarApp.Models;

namespace CalendarApp.Controllers
{
    public class CalendarController : Controller
	{
		#region Controller Actions

		public ActionResult Year(int year)
        {
			DateTime userDate = TimeUtility.BuildDate(year, -1, -1);
			DateTime startRange = TimeUtility.BuildDate(userDate.Year, 1, 1);
			DateTime endRange = TimeUtility.BuildDate(userDate.Year, 12, 31);

			this.ViewData["UserDate"] = userDate;

			CalendarDataContext DB = new CalendarDataContext();
			var items =
				from evt in DB.Events
				where (evt.Starting >= startRange && evt.Starting <= endRange) ||
					(evt.Ending >= startRange && evt.Ending <= endRange)
				select evt;

			this.ViewData["ListData"] =
				new
				{
					date = userDate,
					items = items
				};

			return View();
        }

		public ActionResult Month(int year, int month)
		{
			DateTime userDate = TimeUtility.BuildDate(year, month, -1);

			this.ViewData["UserDate"] = userDate;

			return View();
		}

		public ActionResult Day(int year, int month, int day)
		{
			DateTime userDate = TimeUtility.BuildDate(year, month, day);

			this.ViewData["UserDate"] = userDate;

			return View();
		}

		#endregion Controller Actions
	}
}
