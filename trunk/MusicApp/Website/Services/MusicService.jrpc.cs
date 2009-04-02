using System;
using System.Linq;

using JsonFx.JsonRpc;
using MusicApp.Model;

namespace MusicApp.Services
{
	[JsonService(Namespace = "Music", Name = "MusicService")]
	public class MusicService
	{
		#region Constants

		private const string WikipediaFormat = "http://en.wikipedia.org/wiki/{0}";

		#endregion Constants

		#region Service Methods

		[JsonMethod(Name = "getSummary")]
		public object GetSummary(int start, int count)
		{
			MusicDataContext DB = new MusicDataContext();
			return new
			{
				count = count,
				start = start,
				total = DB.Artists.Count(),
				artists =
					(from artist in DB.Artists
					 let artistCount = DB.Artists.Count()
					 orderby  (artist.SortName ?? artist.ArtistName)
					 let members = artist.Members
					 let startYear =
						(from member in members
						 orderby member.StartYear
						 select member.StartYear).FirstOrDefault()
					 let endYear =
						(from member in members
						 orderby member.EndYear.GetValueOrDefault(9999)
						 select member.EndYear).Skip(members.Count()-1).FirstOrDefault()
					 let currentMembers =
						(from member in members
						 where member.EndYear.GetValueOrDefault(9999) == endYear.GetValueOrDefault(9999)
						 select member).Count()
					select new
					{
						id = artist.ArtistID,
						name = artist.ArtistName,
						startYear = startYear,
						endYear = endYear,
						totalMembers = members.Count(),
						currentMembers = currentMembers
					}).Skip(start).Take(count)
			};
		}

		[JsonMethod(Name = "getArtist")]
		public object GetArtist(long artistID)
		{
			MusicDataContext DB = new MusicDataContext();
			return
				from artist in DB.Artists
				where artist.ArtistID == artistID
				let members = artist.Members
				let startYear =
					(from member in members
					 orderby member.StartYear
					 select member.StartYear).FirstOrDefault()
				let endYear =
					(from member in members
					 orderby member.EndYear.GetValueOrDefault(9999)
					 select member.EndYear).Skip(members.Count()-1).FirstOrDefault()
				let currentMembers =
					(from member in members
					 where member.EndYear.GetValueOrDefault(9999) == endYear.GetValueOrDefault(9999)
					 select member).Count()
				let genreData =
					from ag in DB.ArtistGenres
					where ag.ArtistID == artistID
					from genre in DB.Genres
					where genre.GenreID == ag.GenreID
					select new
					{
						id = genre.GenreID,
						name = genre.GenreName
					}
				let memberData =
					from member in members
					select new
					{
						id = member.MemberID,
						firstName = member.FirstName,
						lastName = member.LastName,
						startYear = member.StartYear,
						endYear = member.EndYear,
						instruments = member.Instruments.Split(','),
						url = String.Format(WikipediaFormat, member.WikipediaKey)
					}
				select new
				{
					id = artist.ArtistID,
					name = artist.ArtistName,
					startYear = startYear,
					endYear = endYear,
					genres = genreData,
					totalMembers = members.Count(),
					currentMembers = currentMembers,
					members = memberData
				};
		}

		#endregion Service Methods
	}
}