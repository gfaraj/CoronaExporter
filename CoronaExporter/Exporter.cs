using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squadventure.Corona
{
	public class Exporter
	{
		public Exporter()
		{
		}

		public string AppDirectory 
		{ 
			get; set; 
		}

		public string ImageSuffix
		{
			get; set;
		}

		public string TargetPath
		{
			get; set;
		}

		public void Export()
		{
			Debug.Assert(!string.IsNullOrEmpty(AppDirectory) && !string.IsNullOrEmpty(TargetPath));
			Debug.Assert(Directory.Exists(AppDirectory));

			CopyDirectory(AppDirectory, TargetPath);

			ProcessDirectory(TargetPath);
		}

		private static void CopyDirectory(string sourceDirName, string destDirName)
		{
			var dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}

			if (Directory.Exists(destDirName))
			{
				Directory.Delete(destDirName, true);
			}

			Directory.CreateDirectory(destDirName);
			
			foreach (var file in dir.GetFiles())
			{
				file.CopyTo(Path.Combine(destDirName, file.Name), false);
			}

			foreach (var subdir in dir.GetDirectories())
			{
				CopyDirectory(subdir.FullName, Path.Combine(destDirName, subdir.Name));
			}
		}

		private bool IsImageFile(string path)
		{
			var extension = Path.GetExtension(path).ToLower();
			if (extension == ".png" || extension == ".jpg")
			{
				return true;
			}
			return false;
		}
		
		private void ProcessDirectory(string path)
		{
			foreach (string file in Directory.GetFiles(path))
			{
				if (IsImageFile(file))
				{
					string fileName = Path.GetFileNameWithoutExtension(file);

					if (path == TargetPath && fileName.StartsWith("Icon"))
					{
						continue;
					}					

					if (fileName.Contains("@"))
					{
						if (ImageSuffix == "" || !fileName.EndsWith(ImageSuffix))
						{
							File.Delete(file);
						}
					}
					else if (ImageSuffix.Length > 0)
					{
						if (Directory.GetFiles(path, fileName + ImageSuffix + Path.GetExtension(file), SearchOption.TopDirectoryOnly).Length > 0)
						{
							File.Delete(file);
						}
					}
				}
			}

			foreach (string subDir in Directory.GetDirectories(path))
			{
				ProcessDirectory(subDir);
			}
		}
	}
}
