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
		public object Browse(string path)
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
		[DefaultValue(null)]
		[JsonName("name")]
		public string Name;

		[DefaultValue(null)]
		[JsonName("path")]
		public string Path;

		[DefaultValue(false)]
		[JsonName("isFolder")]
		public bool IsFolder;

		[DefaultValue(false)]
		[JsonName("isSpecial")]
		public bool IsSpecial;

		//[DefaultValue(null)]
		//[JsonName("mimeType")]
		//public string MimeType;

		//[DefaultValue(null)]
		//[JsonName("fileType")]
		//public string FileType;

		//[DefaultValue(null)]
		//[JsonName("dateCreated")]
		//public Nullable<DateTime> DateCreated;

		//[DefaultValue(null)]
		//[JsonName("dateModified")]
		//public Nullable<DateTime> DateModified;

		//[DefaultValue(null)]
		//[JsonName("dateAccessed")]
		//public Nullable<DateTime> DateAccessed;

		[DefaultValue(null)]
		[JsonSpecifiedProperty("HasPlaylist")]
		[JsonName("playlist")]
		public string Playlist;

		[JsonIgnore]
		public bool HasPlaylist
		{
			get { return !String.IsNullOrEmpty(this.Playlist); }
		}

		[JsonName("children")]
		[JsonSpecifiedProperty("HasChildren")]
		public List<BrowseNode> Children = new List<BrowseNode>();

		[JsonIgnore]
		public bool HasChildren
		{
			get { return this.Children.Count > 0; }
		}
	}
}