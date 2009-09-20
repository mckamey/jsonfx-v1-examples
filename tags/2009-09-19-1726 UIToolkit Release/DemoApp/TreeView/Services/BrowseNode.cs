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
		private MimeCategory category;

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
			BrowseNode node = new BrowseNode();
			node.Name = info.Name;
			node.Path = BrowseService.GetVirtualPath(info.FullName);

			if ((info.Attributes&FileAttributes.Directory) == FileAttributes.Directory)
			{
				node.Category = MimeCategory.Folder;
			}
			else
			{
				MimeType mime = MimeTypes.GetByExtension(info.Extension);
				if (mime != null)
				{
					node.Category = mime.Category;
				}
			}

			return node;
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