using System;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Compilation;
using System.Text.RegularExpressions;

using MimeUtils;
using JsonFx.Json;
using JsonFx.JsonRpc;

namespace StarterKit
{
	[JsonService(Namespace="Browse", Name="Proxy")]
	public class BrowseService
	{
		#region Constants

		private static readonly string PhysicalRoot;
		private static readonly string VirtualRoot;

		private static readonly Regex Regex_InvalidChars = new Regex(@"[&#%=]", RegexOptions.Compiled);
		private static readonly Regex Regex_EncodedChars = new Regex(@"_0x(?<charCode>[0-9]+)_", RegexOptions.Compiled|RegexOptions.ExplicitCapture);

		#endregion Constants

		#region Init

		/// <summary>
		/// CCtor
		/// </summary>
		static BrowseService()
		{
			BrowseService.PhysicalRoot = HttpRuntime.AppDomainAppPath;
			BrowseService.VirtualRoot = HttpRuntime.AppDomainAppVirtualPath;
		}

		#endregion Init

		#region Service Methods

		//[JsonMethod("view")]
		public object View(string path)
		{
			path = BrowseService.GetPhysicalPath(path);

			FileInfo file = new FileInfo(path);
			if (!file.Exists)
			{
				throw new FileNotFoundException("File does not exist.");
			}

			// TODO: create different transformations (e.g. code pretty print)
			using (StreamReader reader = file.OpenText())
			{
				return reader.ReadToEnd();
			}
		}

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
				return BrowseNode.Create(file, true);
			}

			BrowseNode node = BrowseNode.Create(target, false);
			if (isRoot)
			{
				node.Name = String.Empty;
			}

			FileSystemInfo[] children = target.GetFileSystemInfos();
			foreach (FileSystemInfo child in children)
			{
				BrowseNode childNode = BrowseNode.Create(child, false);
				node.Children.Add(childNode);
			}

			return node;
		}

		#endregion Service Methods

		#region Utility Methods

		public static string GetVirtualPath(string path)
		{
			path = path.Substring(BrowseService.PhysicalRoot.Length-1).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

			path = BrowseService.ScrubPath(path);

			path = BrowseService.VirtualRoot + path;

			return path;
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

			path = BrowseService.RepairPath(path);

			path = Path.Combine(BrowseService.PhysicalRoot, path);
			return path;
		}

		private static string ScrubPath(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				return String.Empty;
			}

			return BrowseService.Regex_InvalidChars.Replace(path, BrowseService.InvalidCharReplace);
		}

		private static string RepairPath(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				return String.Empty;
			}

			return BrowseService.Regex_EncodedChars.Replace(path, BrowseService.EncodedCharReplace);
		}

		private static string InvalidCharReplace(Match match)
		{
			if (!match.Success || String.IsNullOrEmpty(match.Value) || (match.Value.Length < 1))
			{
				return match.Value;
			}

			return String.Format("_0x{0:x2}_", (int)match.Value[0]);
		}

		private static string EncodedCharReplace(Match match)
		{
			int charCode;
			if (!Int32.TryParse(match.Groups["charCode"].Value, NumberStyles.HexNumber,
				CultureInfo.InvariantCulture, out charCode))
			{
				return match.Value;
			}

			return ((char)charCode).ToString();
		}

		#endregion Utility Methods
	}

	public class BrowseNode
	{
		#region Fields

		private string name;
		private string path;
		private long bytes = 0L;
		private bool lazyLoad = false;
		private bool isSpecial = false;
		private MimeCategory category = MimeCategory.Unknown;
		private string fileType;
		private string mimeType;
		private Nullable<DateTime> dateCreated;
		private Nullable<DateTime> dateModified;

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
			set
			{
				this.path = value;
				this.EnsureFolderPath();
			}
		}

		[DefaultValue(0L)]
		[JsonName("bytes")]
		public long Bytes
		{
			get { return this.bytes; }
			set { this.bytes = value; }
		}

		[DefaultValue(false)]
		[JsonName("isSpecial")]
		public bool IsSpecial
		{
			get { return this.isSpecial; }
			set { this.isSpecial = value; }
		}

		[DefaultValue(false)]
		[JsonName("lazyLoad")]
		public bool LazyLoad
		{
			get { return this.lazyLoad; }
			set { this.lazyLoad = value; }
		}

		[JsonName("category")]
		public MimeCategory Category
		{
			get { return this.category; }
			set
			{
				this.category = value;
				this.EnsureFolderPath();
			}
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

		public static BrowseNode Create(FileSystemInfo info, bool addDetails)
		{
			BrowseNode node = new BrowseNode();
			node.LazyLoad = !addDetails;
			node.SetName(info.Name);
			node.Path = BrowseService.GetVirtualPath(info.FullName);
			if ((info.Attributes&FileAttributes.Directory) == FileAttributes.Directory)
			{
				node.Category = MimeCategory.Folder;
			}
			else
			{
				if (info is FileInfo)
				{
					node.bytes = ((FileInfo)info).Length;
				}
				MimeType mime = MimeTypes.GetByExtension(info.Extension);
				if (mime != null)
				{
					node.Category = mime.Category;
					if (addDetails)
					{
						node.FileType = mime.Name;
						node.MimeType = mime.ContentType;
					}
				}
			}
			if (addDetails)
			{
				node.DateCreated = info.CreationTimeUtc;
				node.DateModified = info.LastWriteTimeUtc;
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

		private void EnsureFolderPath()
		{
			if (this.category == MimeCategory.Folder &&
				!this.path.EndsWith(System.IO.Path.AltDirectorySeparatorChar.ToString()))
			{
				this.path += System.IO.Path.AltDirectorySeparatorChar;
			}
		}

		#endregion Methods
	}
}