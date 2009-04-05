using System;
using System.Linq;

using JsonFx.JsonRpc;
using MusicApp.Model;

namespace MusicApp.Services
{
	[JsonService(Namespace = "Music", Name = "MusicService")]
	public class MusicService
	{
		#region Service Methods

		//[JsonMethod(Name = "getSummary")]
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
					 let members =
						from member in DB.Members
						where member.ArtistID == artist.ArtistID
						select member
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
						ArtistID = artist.ArtistID,
						Name = artist.ArtistName,
						StartYear = startYear,
						EndYear = endYear,
						TotalMembers = members.Count(),
						CurrentMembers = currentMembers
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
				let members =
					from member in DB.Members
					where member.ArtistID == artist.ArtistID
					select member
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
						GenreID = genre.GenreID,
						GenreName = genre.GenreName
					}
				let memberData =
					from member in members
					select new
					{
						MemberID = member.MemberID,
						FirstName = member.FirstName,
						LastName = member.LastName,
						StartYear = member.StartYear,
						EndYear = member.EndYear,
						Instruments = member.Instruments,
						WikipediaKey = member.WikipediaKey
					}
				select new
				{
					ArtistID = artist.ArtistID,
					ArtistName = artist.ArtistName,
					StartYear = startYear,
					EndYear = endYear,
					Genres = genreData,
					TotalMembers = members.Count(),
					CurrentMembers = currentMembers,
					WikipediaKey = artist.WikipediaKey,
					Members = memberData
				};
		}

		//[JsonMethod(Name="getMember")]
		public object GetMember(long memberID)
		{
			MusicDataContext DB = new MusicDataContext();
			return
				from m in DB.Members
				where m.MemberID == memberID
				select new
				{
					MemberID = m.MemberID,
					FirstName = m.FirstName,
					LastName = m.LastName,
					StartYear = m.StartYear,
					EndYear = m.EndYear,
					Instruments = m.Instruments,
					WikipediaKey = m.WikipediaKey
				};
		}

		#endregion Service Methods
	}
}