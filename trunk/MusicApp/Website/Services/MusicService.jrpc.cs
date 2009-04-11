using System;
using System.Linq;

using JsonFx.JsonRpc;
using MusicApp.Model;

namespace MusicApp.Services
{
	/// <summary>
	/// This is a pretty simple JSON-RPC service which performs
	/// CRUD operations for MusicApp.  It leverages Linq-to-SQL
	/// as a DAL. For simplicity this service directly uses the
	/// strongly typed Linq-to-SQL entities as DTOs serializing
	/// them back and forth as JSON to the client.
	/// </summary>
	[JsonService(Namespace="Music", Name="Service")]
	public class MusicService
	{
		#region Artist CRUD Methods

		[JsonMethod(Name="getArtists")]
		public object GetArtists()
		{
			MusicDataContext DB = new MusicDataContext();

			// top-level anonymous object holding the data to bind
			return new
			{
				GenreName = "all",
				Genres = DB.Genres,
				Artists =
					from artist in DB.Artists
					select artist
			};
		}

		[JsonMethod(Name="getGenre")]
		public object GetGenre(long genreID)
		{
			MusicDataContext DB = new MusicDataContext();

			// the genre being queried
			var genre =
				(from g in DB.Genres
				 where g.GenreID == genreID
				 select g).SingleOrDefault();

			// all artists in this genre
			var artists =
				from ag in DB.ArtistGenres
				where ag.GenreID == genreID
				from a in DB.Artists
				where a.ArtistID == ag.ArtistID
				select a;

			// all genres to which those artists belong
			var genres =
				(from a in artists
				from ag in DB.ArtistGenres
				where a.ArtistID == ag.ArtistID
				from g in DB.Genres
				where g.GenreID == ag.GenreID
				select g).Distinct();

			// top-level anonymous object holding the data to bind
			return new
			{
				GenreName = genre.GenreName,
				Genres = genres,
				Artists = artists
			};
		}

		[JsonMethod(Name="saveArtist")]
		public Artist SaveArtist(Artist artist)
		{
			if (artist == null)
			{
				throw new ArgumentNullException("artist", "artist was null.");
			}

			MusicDataContext DB = new MusicDataContext();

			if (artist.ArtistID > 0)
			{
				// update an existing artist
				DB.Artists.Attach(artist, true);
			}
			else
			{
				// create a new artist
				DB.Artists.InsertOnSubmit(artist);
			}

			// serialize the saved member back to the client
			DB.SubmitChanges();

			return artist;
		}

		[JsonMethod("deleteArtist")]
		public void DeleteArtist(long artistID)
		{
			if (artistID <= 0)
			{
				throw new ArgumentOutOfRangeException("artistID", "Invalid ArtistID.");
			}

			MusicDataContext DB = new MusicDataContext();

			// find and delete the artist
			var artist = DB.Artists.Single(a => a.ArtistID == artistID);
			DB.Artists.DeleteOnSubmit(artist);
			DB.SubmitChanges();
		}

		#endregion Artist CRUD Methods

		#region Member CRUD Methods

		[JsonMethod(Name="getMembers")]
		public object GetMembers(long artistID)
		{
			MusicDataContext DB = new MusicDataContext();

			// serialize an object graph to the client
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

				// top-level anonymous object holding the data to bind
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

		[JsonMethod("saveMember")]
		public Member SaveMember(Member member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member", "Member was null.");
			}

			MusicDataContext DB = new MusicDataContext();
			if (member.MemberID > 0)
			{
				// update an existing member
				DB.Members.Attach(member, true);
			}
			else
			{
				// create a new member
				DB.Members.InsertOnSubmit(member);
			}
			DB.SubmitChanges();

			// serialize the saved member back to the client
			return member;
		}

		[JsonMethod("deleteMember")]
		public void DeleteMember(long memberID)
		{
			if (memberID <= 0)
			{
				throw new ArgumentOutOfRangeException("memberID", "Invalid MemberID.");
			}

			MusicDataContext DB = new MusicDataContext();

			// find and delete a member
			var member = DB.Members.Single(m => m.MemberID == memberID);
			DB.Members.DeleteOnSubmit(member);
			DB.SubmitChanges();
		}

		#endregion Member CRUD Methods
	}
}