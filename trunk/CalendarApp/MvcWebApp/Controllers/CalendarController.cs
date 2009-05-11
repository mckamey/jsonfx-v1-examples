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

			var items = new CalendarService().Search(userDate, SearchRange.Year);
			this.BuildViewData(userDate, items);

			return View();
        }

		public ActionResult Month(int year, int month)
		{
			DateTime userDate = TimeUtility.BuildDate(year, month, -1);

			var items = new CalendarService().Search(userDate, SearchRange.Month);
			this.BuildViewData(userDate, items);

			return View();
		}

		public ActionResult Day(int year, int month, int day)
		{
			DateTime userDate = TimeUtility.BuildDate(year, month, day);

			var items = new CalendarService().Search(userDate, SearchRange.Day);
			this.BuildViewData(userDate, items);

			return View();
		}

		#endregion Controller Actions

		#region Utility Methods

		private void BuildViewData(DateTime userDate, object viewData)
		{
			this.ViewData["DisplayDate"] = userDate;
			this.ViewData["ViewData"] = viewData;
		}

		#endregion Utility Methods
	}
}
