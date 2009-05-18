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
		Week,
		Month,
		Year
	}

	[JsonService(Namespace="Calendar", Name="Service")]
	public class CalendarService
	{
		[JsonMethod(Name="searchDate")]
		public object Search(DateTime date, SearchRange range, int start, int count)
		{
			switch (range)
			{
				case SearchRange.Day:
				{
					DateTime startRange = TimeUtility.BuildDate(date.Year, date.Month, date.Day, 0, 0, 0);
					DateTime endRange = TimeUtility.BuildDate(date.Year, date.Month, date.Day, 23, 59, 59);
					return this.Search(date, startRange, endRange, start, count);
				}
				case SearchRange.Week:
				{
					int offset = (int)date.DayOfWeek;
					TimeSpan startOffset = TimeSpan.FromDays(offset);
					TimeSpan endOffset = TimeSpan.FromDays((int)DayOfWeek.Saturday - offset);

					DateTime startRange = TimeUtility.BuildDate(date.Year, date.Month, date.Day, 0, 0, 0).Subtract(startOffset);
					DateTime endRange = TimeUtility.BuildDate(date.Year, date.Month, date.Day, 23, 59, 59).Add(endOffset);
					return this.Search(date, startRange, endRange, start, count);
				}
				case SearchRange.Month:
				{
					DateTime startRange = TimeUtility.BuildDate(date.Year, date.Month, 1, 0, 0, 0);
					DateTime endRange = TimeUtility.BuildDate(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
					return this.Search(date, startRange, endRange, start, count);
				}
				case SearchRange.Year:
				default:
				{
					DateTime startRange = TimeUtility.BuildDate(date.Year, 1, 1, 0, 0, 0);
					DateTime endRange = TimeUtility.BuildDate(date.Year, 12, 31, 23, 59, 59);
					return this.Search(date, startRange, endRange, start, count);
				}
			}
		}

		[JsonMethod(Name="searchRange")]
		public object Search(DateTime selectedDate, DateTime startRange, DateTime endRange, int start, int count)
		{
			selectedDate = TimeUtility.ToUtcTimeZone(selectedDate);
			startRange = TimeUtility.ToUtcTimeZone(startRange);
			endRange = TimeUtility.ToUtcTimeZone(endRange);

			CalendarDataContext DB = new CalendarDataContext();

			var items =
				(from evt in DB.Events
				where
					(evt.Starting >= startRange && evt.Starting <= endRange) ||
					(evt.Ending >= startRange && evt.Ending <= endRange)
				orderby evt.Starting
				select new
				{
					Label = evt.Label,
					Details = evt.Details,
					Starting = evt.Starting,
					Ending = evt.Ending
				}).Skip(start).Take(count);

			return new
			{
				SelectedDate = selectedDate,
				StartRange = startRange,
				EndRange = endRange,
				Items = items
			};
		}
	}
}