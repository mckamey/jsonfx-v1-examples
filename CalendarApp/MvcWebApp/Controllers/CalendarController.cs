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
			this.ViewData["UserDate"] = TimeUtility.BuildDate(year, -1, -1);

			return View();
        }

		public ActionResult Month(int year, int month)
		{
			this.ViewData["UserDate"] = TimeUtility.BuildDate(year, month, -1);

			return View();
		}

		public ActionResult Day(int year, int month, int day)
		{
			this.ViewData["UserDate"] = TimeUtility.BuildDate(year, month, day);

			return View();
		}

		#endregion Controller Actions
	}
}
