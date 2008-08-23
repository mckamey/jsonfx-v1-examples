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
			path = BrowseService.GetPhysicalPath(path);

			BrowseNode root = new BrowseNode();

			DirectoryInfo target = new DirectoryInfo(path);
			if (!target.Exists)
			{
				throw new FileNotFoundException("Folder does not exist.");
			}

			TrimFolder(root, target.Name);
			root.Path = target.FullName.Substring(PhysicalRoot.Length-1).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			root.IsFolder = true;
			if (!root.Path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
			{
				root.Path += Path.AltDirectorySeparatorChar;
			}

			FileSystemInfo[] children = target.GetFileSystemInfos();
			foreach (FileSystemInfo child in children)
			{
				BrowseNode childNode = new BrowseNode();
				TrimFolder(childNode, child.Name);
				childNode.Path = child.FullName.Substring(PhysicalRoot.Length-1).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				childNode.IsFolder = (child.Attributes&FileAttributes.Directory) == FileAttributes.Directory;
				if (childNode.IsFolder &&
					!childNode.Path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
				{
					childNode.Path += Path.AltDirectorySeparatorChar;
				}

				MimeType mime = MimeTypes.GetByExtension(child.Extension);
				if (mime != null)
				{
					//childNode.MimeType = mime.ContentType;
					//childNode.FileType = mime.Name;

					if (String.IsNullOrEmpty(root.Playlist))
					{
						if (mime.Category == MimeCategory.Audio)
						{
							root.Playlist = Path.Combine(root.Path, "PlayList.m3u");
						}
						else if (mime.Category == MimeCategory.Video)
						{
							root.Playlist = Path.Combine(root.Path, "PlayList.wpl");
						}
					}
				}

				root.Children.Add(childNode);
			}

			return root;
		}

		private static void TrimFolder(BrowseNode node, string folderName)
		{
			if ((folderName.StartsWith("[ ") && folderName.EndsWith(" ]")) ||
				(folderName.StartsWith("( ") && folderName.EndsWith(" )")))
			{
				node.Name = folderName.Substring(2, folderName.Length-4);
				node.IsSpecial = true;
			}
			else
			{
				node.Name = folderName;
			}
		}

		#endregion Service Methods

		#region Utility Methods

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
		private string playlist;
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

		[DefaultValue("")]
		[JsonName("playlist")]
		public string Playlist
		{
			get
			{
				if (this.playlist == null)
				{
					return String.Empty;
				}
				return this.playlist;
			}
			set { this.playlist = value; }
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
	}
}