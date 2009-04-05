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
					 orderby member.EndYear.GetValueOrDefault(9999) // send empty dates to end
					 select member.EndYear).Skip(members.Count()-1).FirstOrDefault()
				let currentMembers =
					(from member in members
					 where member.EndYear.GetValueOrDefault(9999) == endYear.GetValueOrDefault(9999)
					 select member).Count()
				let genres =
					from ag in DB.ArtistGenres
					where ag.ArtistID == artistID
					from genre in DB.Genres
					where genre.GenreID == ag.GenreID
					select genre
				select new
				{
					ArtistID = artist.ArtistID,
					ArtistName = artist.ArtistName,
					StartYear = startYear,
					EndYear = endYear,
					TotalMembers = members.Count(),
					CurrentMembers = currentMembers,
					WikipediaKey = artist.WikipediaKey,
					Genres = genres,
					Members = members
				};
		}

		#endregion Service Methods
	}
}