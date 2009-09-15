using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

using MimeUtils;
using JsonFx.Json;

namespace DemoApp
{
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

		[JsonIgnore]// not used yet
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

		[JsonIgnore]// not used yet
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

		[JsonIgnore]// not used yet
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

		[JsonIgnore]// not used yet
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

		[JsonIgnore]// not used yet
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
		[JsonSpecifiedProperty("ShowChildren")]
		public readonly List<BrowseNode> Children = new List<BrowseNode>();

		[JsonIgnore]
		public bool ShowChildren
		{
			get { return (this.Category == MimeCategory.Folder); }
		}

		#endregion Properties

		#region Methods

		public static BrowseNode Create(FileSystemInfo info)
		{
			bool isFile = (info is FileInfo);

			BrowseNode node = new BrowseNode();
			node.LazyLoad = !isFile;
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
					if (isFile)
					{
						node.FileType = mime.Name;
						node.MimeType = mime.ContentType;
					}
				}
			}

			if (isFile)
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