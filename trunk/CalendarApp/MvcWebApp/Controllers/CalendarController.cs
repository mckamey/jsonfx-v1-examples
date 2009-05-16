using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using CalendarApp.Models;
using CalendarApp.Services;

namespace CalendarApp.Controllers
{
    public class CalendarController : Controller
	{
		#region Controller Actions

		public ActionResult Year(int year)
        {
			DateTime userDate = TimeUtility.BuildDate(year, -1, -1);

			var viewData = new CalendarService().Search(userDate, SearchRange.Year, 0, 10);
			this.ViewData["DisplayDate"] = userDate;
			this.ViewData["ViewData"] = viewData;

			return View();
        }

		public ActionResult Month(int year, int month)
		{
			DateTime userDate = TimeUtility.BuildDate(year, month, -1);

			var viewData = new CalendarService().Search(userDate, SearchRange.Month, 0, 10);
			this.ViewData["DisplayDate"] = userDate;
			this.ViewData["ViewData"] = viewData;

			return View();
		}

		public ActionResult Week(int year, int month, int day)
		{
			DateTime userDate = TimeUtility.BuildDate(year, month, day);

			var viewData = new CalendarService().Search(userDate, SearchRange.Week, 0, 10);
			this.ViewData["DisplayDate"] = userDate;
			this.ViewData["ViewData"] = viewData;

			return View();
		}

		public ActionResult Day(int year, int month, int day)
		{
			DateTime userDate = TimeUtility.BuildDate(year, month, day);

			var viewData = new CalendarService().Search(userDate, SearchRange.Day, 0, 10);
			this.ViewData["DisplayDate"] = userDate;
			this.ViewData["ViewData"] = viewData;

			return View();
		}

		#endregion Controller Actions
	}
}
