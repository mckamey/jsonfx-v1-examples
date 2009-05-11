using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JsonFx.Json;
using JsonFx.JsonRpc;
using CalendarApp.Models;

namespace CalendarApp.Services
{
	public enum SearchRange
	{
		None,
		Day,
		Month,
		Year
	}

	[JsonService(Namespace="Calendar", Name="Service")]
	public class CalendarService
	{
		[JsonMethod(Name="searchDate")]
		public object Search(DateTime date, SearchRange range)
		{
			switch (range)
			{
				case SearchRange.Day:
				{
					DateTime startRange = TimeUtility.BuildDate(date.Year, date.Month, date.Day, 0, 0, 0);
					DateTime endRange = TimeUtility.BuildDate(date.Year, date.Month, date.Day, 23, 59, 59);
					return this.Search(startRange, endRange);
				}
				case SearchRange.Month:
				{
					DateTime startRange = TimeUtility.BuildDate(date.Year, date.Month, 1, 0, 0, 0);
					DateTime endRange = TimeUtility.BuildDate(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
					return this.Search(startRange, endRange);
				}
				case SearchRange.Year:
				default:
				{
					DateTime startRange = TimeUtility.BuildDate(date.Year, 1, 1, 0, 0, 0);
					DateTime endRange = TimeUtility.BuildDate(date.Year, 12, 31, 23, 59, 59);
					return this.Search(startRange, endRange);
				}
			}
		}

		[JsonMethod(Name="searchRange")]
		public object Search(DateTime startRange, DateTime endRange)
		{
			CalendarDataContext DB = new CalendarDataContext();

			var items =
				from evt in DB.Events
				where
					(evt.Starting >= startRange && evt.Starting <= endRange) ||
					(evt.Ending >= startRange && evt.Ending <= endRange)
				orderby evt.Starting
				select evt;

			return new
			{
				StartRange = startRange,
				EndRange = endRange,
				Items = items
			};
		}
	}
}