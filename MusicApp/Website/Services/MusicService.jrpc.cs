using System;
using System.Collections.Generic;
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
		#region Genre CRUD Methods

		[JsonMethod(Name="getGenres")]
		public IEnumerable<Genre> GetGenres()
		{
			MusicDataContext DB = new MusicDataContext();

			return
				from genre in DB.Genres
				select genre;
		}

		[JsonMethod(Name="addGenre")]
		public Genre AddGenre(string name)
		{
			MusicDataContext DB = new MusicDataContext();

			Genre genre = new Genre();
			genre.GenreName = name;

			// create a new genre
			DB.Genres.InsertOnSubmit(genre);

			// serialize the saved member back to the client
			DB.SubmitChanges();

			return genre;
		}

		[JsonMethod(Name="getArtistGenres")]
		public IEnumerable<long> GetArtistGenres(long artistID)
		{
			MusicDataContext DB = new MusicDataContext();

			return
				 from ag in DB.ArtistGenres
				 where artistID == ag.ArtistID
				 select ag.GenreID;
		}

		[JsonMethod(Name="setArtistGenres")]
		public void SetArtistGenres(long artistID, List<long> genres)
		{
			if (artistID < 0)
			{
				throw new ArgumentNullException("artistID", "artistID is invalid.");
			}
			if (genres == null)
			{
				// ensure list
				genres = new List<long>(1);
			}

			MusicDataContext DB = new MusicDataContext();

			var artistGenres =
				from ag in DB.ArtistGenres
				where ag.ArtistID == artistID
				select ag;

			var toAdd =
				from genreID in genres
				where !(from ag in artistGenres
						select ag.GenreID)
						.Contains(genreID)
				select new ArtistGenre
				{
					GenreID = genreID,
					ArtistID = artistID
				};

			// add all new artist-genre relations
			DB.ArtistGenres.InsertAllOnSubmit(toAdd);

			var toRemove =
				from ag in artistGenres
				where !(from genreID in genres
						select genreID)
						.Contains(ag.GenreID)
				select ag;

			// remove all missing artist-genre relations
			DB.ArtistGenres.DeleteAllOnSubmit(toRemove);

			// commit to database
			DB.SubmitChanges();
		}

		#endregion Genre CRUD Methods

		#region Artist CRUD Methods

		[JsonMethod(Name="getArtists")]
		public object GetArtists()
		{
			MusicDataContext DB = new MusicDataContext();

			// top-level anonymous object holding the data to bind
			return new
			{
				GenreID = -1L,
				GenreName = "all",
				Genres = DB.Genres,
				Artists = DB.Artists
			};
		}

		[JsonMethod(Name="getGenreDetail")]
		public object GetGenreDetail(long genreID)
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

			// all genres to which the selected artists belong
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
				GenreID = genre.GenreID,
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

			long artistID = artist.ArtistID;
			if (artistID > 0)
			{
				// update an existing artist
				DB.Artists.Attach(artist, true);
			}
			else
			{
				// create a new artist
				DB.Artists.InsertOnSubmit(artist);
			}

			// commit to database
			DB.SubmitChanges();

			// serialize the saved member back to the client
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

			// find and delete the artist (and associated genre relations)
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
					StartYear = ((Nullable<short>)startYear ?? (short)0),
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

			// commit to database
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