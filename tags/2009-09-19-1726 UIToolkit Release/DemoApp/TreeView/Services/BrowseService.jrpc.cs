using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Text.RegularExpressions;

using MimeUtils;
using JsonFx.JsonRpc;

namespace DemoApp
{
	[JsonService(Namespace="DemoApp", Name="BrowseService")]
	public class BrowseService
	{
		#region Constants

		private static readonly string PhysicalRoot;
		private static readonly string VirtualRoot;

		private static readonly Regex Regex_InvalidChars = new Regex(@"[&#%=]", RegexOptions.Compiled);
		private static readonly Regex Regex_EncodedChars = new Regex(@"_0x(?<charCode>[0-9]+)_", RegexOptions.Compiled|RegexOptions.ExplicitCapture);

		private const long MaxFileSize = 1024L*1024L;// cap at 1MB

		#endregion Constants

		#region Init

		/// <summary>
		/// CCtor
		/// </summary>
		static BrowseService()
		{
			BrowseService.PhysicalRoot = HttpRuntime.AppDomainAppPath;
			BrowseService.VirtualRoot = HttpRuntime.AppDomainAppVirtualPath;
			if (String.IsNullOrEmpty(BrowseService.VirtualRoot) ||
				BrowseService.VirtualRoot.Length > 1)
			{
				BrowseService.VirtualRoot += '/';
			}
		}

		#endregion Init

		#region Service Methods

		[JsonMethod("view")]
		public object View(string path)
		{
			path = BrowseService.GetPhysicalPath(path);

			FileInfo info = new FileInfo(path);
			if (!info.Exists)
			{
				throw new FileNotFoundException("File does not exist.");
			}

			MimeType mime = MimeTypes.GetByExtension(info.Extension);
			switch (mime.Category)
			{
				case MimeCategory.Code:
				case MimeCategory.Text:
				case MimeCategory.Xml:
				{
					break;
				}
				default:
				{
					throw new NotSupportedException("Cannot browse this type.");
				}
			}

			if (info.Length > MaxFileSize)
			{
				throw new NotSupportedException("File is too large.");
			}

			// TODO: create different transformations (e.g. pretty print and syntax coloring)
			// if returned as JsonML will display correctly
			using (StreamReader reader = info.OpenText())
			{
				return reader.ReadToEnd();
			}
		}

		[JsonMethod("browse")]
		public BrowseNode Browse(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				path = VirtualRoot;
			}

			bool isRoot = (VirtualRoot).Equals(path);

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

			BrowseNode node = BrowseNode.Create(target);
			if (isRoot)
			{
				node.Name = String.Empty;
			}

			FileSystemInfo[] children = target.GetFileSystemInfos();
			foreach (FileSystemInfo child in children)
			{
				if ((child.Attributes & FileAttributes.System) == FileAttributes.System ||
					(child.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
				{
					continue;
				}

				BrowseNode childNode = BrowseNode.Create(child);
				node.Children.Add(childNode);
			}

			return node;
		}

		#endregion Service Methods

		#region Utility Methods

		public static string GetVirtualPath(string path)
		{
			path = path.Substring(BrowseService.PhysicalRoot.Length-1);
			path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

			path = BrowseService.ScrubPath(path);

			if (path.StartsWith("/"))
			{
				path = path.Substring(1);
			}

			path = BrowseService.VirtualRoot + path;
			return path;
		}

		public static string GetPhysicalPath(string path)
		{
			path = HttpUtility.UrlDecode(path);
			path = BrowseService.RepairPath(path);
			if (path.Length < BrowseService.VirtualRoot.Length)
			{
				throw new ArgumentException("Invalid path.");
			}

			path = path.Substring(BrowseService.VirtualRoot.Length-1);
			path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

			if (path.IndexOf("../") >= 0)
			{
				throw new NotSupportedException("Invalid path.");
			}

			if (path.StartsWith("\\"))
			{
				path = path.Substring(1);
			}

			path = BrowseService.PhysicalRoot + path;
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
}