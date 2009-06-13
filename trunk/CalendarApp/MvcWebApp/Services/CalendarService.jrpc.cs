using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

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
					return this.Counts(date, startRange, endRange);
				}
			}
		}

		[JsonMethod(Name="searchRange")]
		public object Search(DateTime selectedDate, DateTime startRange, DateTime endRange, int start, int count)
		{
			startRange = TimeUtility.ToUtcTimeZone(startRange);
			endRange = TimeUtility.ToUtcTimeZone(endRange);

			CalendarDataContext DB = new CalendarDataContext();
			DB.ObjectTrackingEnabled = false;

			// TODO: establish a more reusuable structure for client-side data
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
				SelectedDate = TimeUtility.ToBrowserTimeZone(selectedDate),
				StartRange = TimeUtility.ToBrowserTimeZone(startRange),
				EndRange = TimeUtility.ToBrowserTimeZone(endRange),
				Items = items
			};
		}

		[JsonMethod(Name="countsRange")]
		public object Counts(DateTime selectedDate, DateTime startRange, DateTime endRange)
		{
			startRange = TimeUtility.ToUtcTimeZone(startRange);
			endRange = TimeUtility.ToUtcTimeZone(endRange);

			CalendarDataContext DB = new CalendarDataContext();

			// TODO: establish a more reusuable structure for client-side data

			// modify this to return daily counts within the range
			// from there a yearly view can be built which shows each month
			// where the day is clickable if count > 0.
			var items =
				(from evt in DB.Events
				 where
					 (evt.Starting >= startRange && evt.Starting <= endRange) ||
					 (evt.Ending >= startRange && evt.Ending <= endRange)
				 orderby evt.Starting
				 group evt by evt.Starting.Date into days
				 select new
				 {
					 Day = days.Key,
					 Count = days.Count()
				 }).ToDictionary(day => day.Day.ToString("yyyy'-'MM'-'dd"), day => day.Count);

			return new
			{
				SelectedDate = TimeUtility.ToBrowserTimeZone(selectedDate),
				StartRange = TimeUtility.ToBrowserTimeZone(startRange),
				EndRange = TimeUtility.ToBrowserTimeZone(endRange),
				Items = items
			};
		}

		[JsonMethod(Name="saveEvent")]
		public Event SaveEvent(Event evt)
		{
			if (evt == null)
			{
				throw new ArgumentNullException("evt", "evt was null.");
			}

			// TODO: set/verify auth here
			evt.CreatedBy = 1L;
			evt.CreatedDate = DateTime.UtcNow;

			CalendarDataContext DB = new CalendarDataContext();

			// TODO: decide what to do about constraints
			if (evt.Label.Length > 50)
			{
				evt.Label = evt.Label.Substring(0, 50);
			}

			if (evt.EventID > 0)
			{
				// update an existing evt
				DB.Events.Attach(evt, true);
			}
			else
			{
				// create a new evt
				DB.Events.InsertOnSubmit(evt);
			}

			try
			{
				// commit to database
				DB.SubmitChanges(ConflictMode.ContinueOnConflict);
			}
			catch (ChangeConflictException e)
			{
				Console.WriteLine(e.Message);
				foreach (ObjectChangeConflict occ in DB.ChangeConflicts)
				{
					occ.Resolve(RefreshMode.KeepChanges);
				}
				DB.SubmitChanges(ConflictMode.FailOnFirstConflict);
			}

			// serialize the saved member back to the client
			return evt;
		}

		[JsonMethod(Name="saveEvents")]
		public List<Event> SaveEvents(List<Event> evts)
		{
			if (evts == null)
			{
				throw new ArgumentNullException("evts", "evts was null.");
			}

			CalendarDataContext DB = new CalendarDataContext();

			foreach (Event evt in evts)
			{
				// TODO: set/verify auth here
				evt.CreatedBy = 1L;
				evt.CreatedDate = DateTime.UtcNow;

				// TODO: decide what to do about constraints
				if (evt.Label.Length > 50)
				{
					evt.Label = evt.Label.Substring(0, 50);
				}

				if (evt.EventID > 0)
				{
					// update an existing evt
					DB.Events.Attach(evt, true);
				}
				else
				{
					// create a new evt
					DB.Events.InsertOnSubmit(evt);
				}
			}

			try
			{
				// commit to database
				DB.SubmitChanges(ConflictMode.ContinueOnConflict);
			}
			catch (ChangeConflictException e)
			{
				Console.WriteLine(e.Message);
				foreach (ObjectChangeConflict occ in DB.ChangeConflicts)
				{
					occ.Resolve(RefreshMode.KeepChanges);
				}
				DB.SubmitChanges(ConflictMode.FailOnFirstConflict);
			}

			// serialize the saved member back to the client
			return evts;
		}
	}
}