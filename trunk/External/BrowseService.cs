using System;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

using MediaLib.Web;
using JsonFx.Json;
using JsonFx.JsonRpc;

namespace MediaLib
{
	[JsonService(Namespace="Browse", Name="Proxy")]
	public class BrowseService
	{
		#region Constants

		private static readonly string PhysicalRoot;

		#endregion Constants

		#region Init

		/// <summary>
		/// CCtor
		/// </summary>
		static BrowseService()
		{
			BrowseService.PhysicalRoot = ConfigurationManager.AppSettings["PhysicalRoot"];
		}

		#endregion Init

		#region Service Methods

		[JsonMethod("browse")]
		public BrowseNode Browse(string path)
		{
			bool isRoot = "/".Equals(path);

			path = BrowseService.GetPhysicalPath(path);

			DirectoryInfo target = new DirectoryInfo(path);
			if (!target.Exists)
			{
				FileInfo file = new FileInfo(path);
				if (!file.Exists)
				{
					throw new FileNotFoundException("Folder does not exist.");
				}
				return BrowseNode.Create(file);
			}

			BrowseNode root = BrowseNode.Create(target);
			if (isRoot)
			{
				root.Name = String.Empty;
			}

			bool hasPlaylist = false;
			FileSystemInfo[] children = target.GetFileSystemInfos();
			foreach (FileSystemInfo child in children)
			{
				BrowseNode childNode = BrowseNode.Create(child);

				MimeType mime = MimeTypes.GetByExtension(child.Extension);
				if (mime != null)
				{
					//childNode.MimeType = mime.ContentType;
					//childNode.FileType = mime.Name;

					if (!hasPlaylist)
					{
						if (mime.Category == MimeCategory.Audio)
						{
							BrowseNode playlist = new BrowseNode();
							playlist.Name = "Playlist";
							playlist.Path = Path.Combine(root.Path, "PlayList.m3u");
							playlist.IsPlaylist = true;
							root.Children.Insert(0, playlist);

							hasPlaylist = true;
						}
						else if (mime.Category == MimeCategory.Video)
						{
							BrowseNode playlist = new BrowseNode();
							playlist.Name = "Playlist";
							playlist.Path = Path.Combine(root.Path, "PlayList.wpl");
							playlist.IsPlaylist = true;
							root.Children.Insert(0, playlist);

							hasPlaylist = true;
						}
					}
				}

				root.Children.Add(childNode);
			}

			return root;
		}

		#endregion Service Methods

		#region Utility Methods

		public static string GetVirtualPath(string path)
		{
			return path.Substring(BrowseService.PhysicalRoot.Length-1).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}

		public static string GetPhysicalPath(string path)
		{
			if (path == null)
			{
				path = String.Empty;
			}

			path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			path = HttpUtility.UrlDecode(path);
			if (path.IndexOf("..\\") >= 0)
			{
				throw new NotSupportedException("Invalid path.");
			}

			int i = 0;
			for (; i<path.Length; i++)
			{
				if (path[i] != '.' && path[i] != '\\')
				{
					break;
				}
			}
			path = path.Substring(i);
			path = Path.Combine(BrowseService.PhysicalRoot, path);
			return path;
		}

		#endregion Utility Methods
	}

	public class BrowseNode
	{
		#region Fields

		private string name;
		private string path;
		private bool isPlaylist = false;
		private bool isFolder = false;
		private bool isSpecial = false;
		private string mimeType;
		private string fileType;
		private Nullable<DateTime> dateCreated;
		private Nullable<DateTime> dateModified;
		private Nullable<DateTime> dateAccessed;

		#endregion Fields

		#region Properties

		[DefaultValue(null)]
		[JsonName("name")]
		public string Name
		{
			get
			{
				if (this.name == null)
				{
					return String.Empty;
				}
				return this.name;
			}
			set { this.name = value; }
		}

		[DefaultValue("")]
		[JsonName("path")]
		public string Path
		{
			get
			{
				if (this.path == null)
				{
					return String.Empty;
				}
				return this.path;
			}
			set { this.path = value; }
		}

		[DefaultValue(false)]
		[JsonName("isFolder")]
		public bool IsFolder
		{
			get { return this.isFolder; }
			set { this.isFolder = value; }
		}

		[DefaultValue(false)]
		[JsonName("isSpecial")]
		public bool IsSpecial
		{
			get { return this.isSpecial; }
			set { this.isSpecial = value; }
		}

		[DefaultValue(false)]
		[JsonName("isPlaylist")]
		public bool IsPlaylist
		{
			get { return this.isPlaylist; }
			set { this.isPlaylist = value; }
		}

		[DefaultValue("")]
		[JsonName("mimeType")]
		public string MimeType
		{
			get
			{
				if (this.mimeType == null)
				{
					return String.Empty;
				}
				return this.mimeType;
			}
			set { this.mimeType = value; }
		}

		[DefaultValue("")]
		[JsonName("fileType")]
		public string FileType
		{
			get
			{
				if (this.fileType == null)
				{
					return String.Empty;
				}
				return this.fileType;
			}
			set { this.fileType = value; }
		}

		[JsonName("dateCreated")]
		[JsonSpecifiedProperty("HasDateCreated")]
		public DateTime DateCreated
		{
			get
			{
				if (!this.dateCreated.HasValue)
				{
					return DateTime.MinValue;
				}
				return this.dateCreated.Value;
			}
			set { this.dateCreated = value; }
		}

		[JsonIgnore]
		public bool HasDateCreated
		{
			get { return this.dateCreated.HasValue; }
		}

		[JsonName("dateModified")]
		[JsonSpecifiedProperty("HasDateModified")]
		public DateTime DateModified
		{
			get
			{
				if (!this.dateModified.HasValue)
				{
					return DateTime.MinValue;
				}
				return this.dateModified.Value;
			}
			set { this.dateModified = value; }
		}

		[JsonIgnore]
		public bool HasDateModified
		{
			get { return this.dateModified.HasValue; }
		}

		[JsonName("dateAccessed")]
		[JsonSpecifiedProperty("HasDateAccessed")]
		public DateTime DateAccessed
		{
			get
			{
				if (!this.dateAccessed.HasValue)
				{
					return DateTime.MinValue;
				}
				return this.dateAccessed.Value;
			}
			set { this.dateAccessed = value; }
		}

		[JsonIgnore]
		public bool HasDateAccessed
		{
			get { return this.dateAccessed.HasValue; }
		}

		[JsonName("children")]
		[JsonSpecifiedProperty("HasChildren")]
		public readonly List<BrowseNode> Children = new List<BrowseNode>();

		[JsonIgnore]
		public bool HasChildren
		{
			get { return this.Children.Count > 0; }
		}

		#endregion Properties

		#region Methods

		public static BrowseNode Create(FileSystemInfo info)
		{
			BrowseNode node = new BrowseNode();
			node.SetName(info.Name);
			node.Path = BrowseService.GetVirtualPath(info.FullName);
			node.IsFolder = (info.Attributes&FileAttributes.Directory) == FileAttributes.Directory;
			if (node.IsFolder && !node.Path.EndsWith(System.IO.Path.AltDirectorySeparatorChar.ToString()))
			{
				node.Path += System.IO.Path.AltDirectorySeparatorChar;
			}
			return node;
		}

		private void SetName(string folderName)
		{
			if ((folderName.StartsWith("[ ") && folderName.EndsWith(" ]")) ||
				(folderName.StartsWith("( ") && folderName.EndsWith(" )")))
			{
				this.Name = folderName.Substring(2, folderName.Length-4);
				this.IsSpecial = true;
			}
			else
			{
				this.Name = folderName;
			}
		}

		#endregion Methods
	}
}