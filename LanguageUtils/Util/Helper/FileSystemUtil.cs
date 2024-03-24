using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace MSHC.Util.Helper
{
	public static class FileSystemUtil
	{
		public static IEnumerable<string> EnumerateFilesDeep(string baseFolder, int remainingDepth)
		{
			if (remainingDepth == 0) yield break;

			foreach (var f in Directory.EnumerateFiles(baseFolder)) yield return f;

			foreach (var d in Directory.EnumerateDirectories(baseFolder))
			{
				foreach (var f in EnumerateFilesDeep(d, remainingDepth-1)) yield return f;
			}
		}
		
		public static IEnumerable<string> EnumerateFilesDeep(string baseFolder, int remainingDepth, string[] excludedDirectories)
		{
			if (remainingDepth == 0) yield break;

			foreach (var f in Directory.EnumerateFiles(baseFolder)) yield return f;

			foreach (var d in Directory.EnumerateDirectories(baseFolder))
			{
				if (excludedDirectories.Any(ed => ed == Path.GetFileName(d))) continue;

				foreach (var f in EnumerateFilesDeep(d, remainingDepth-1, excludedDirectories)) yield return f;
			}
		}

		public static void DeleteFileAndFolderIfEmpty(string baseFolder, string file)
		{
			var fi = new FileInfo(file);
			if (fi.Exists && fi.IsReadOnly) fi.IsReadOnly = false;

			File.Delete(file);

			DeleteFolderIfEmpty(baseFolder, Path.GetDirectoryName(file));
		}
		
		public static void DeleteFolderIfEmpty(string baseFolder, string folder)
		{
			var p1 = Path.GetFullPath(baseFolder).TrimEnd(Path.DirectorySeparatorChar).ToLower();
			var p2 = Path.GetFullPath(folder).TrimEnd(Path.DirectorySeparatorChar).ToLower();
			if (p1 == p2) return;
			if (p1.Count(c => c == Path.DirectorySeparatorChar) >= p2.Count(c => c == Path.DirectorySeparatorChar)) return;

			if (Directory.EnumerateFileSystemEntries(folder).Any()) return;

			Directory.Delete(folder);
		}		

		public static string MakePathRelative(string fromPath, string baseDir)
		{
			string pathSep = System.IO.Path.DirectorySeparatorChar.ToString();
			
			string[] p1 = Regex.Split(fromPath, @"[\\/]").Where(x => x.Length != 0).ToArray();
			string[] p2 = Regex.Split(baseDir, @"[\\/]").Where(x => x.Length != 0).ToArray();

			int i = 0;
			for (; i < p1.Length && i < p2.Length; i++)
			{
				if (string.Compare(p1[i], p2[i], StringComparison.OrdinalIgnoreCase) != 0) break;
			}

			return string.Join(pathSep, p1.Skip(i));
		}

		public static IEnumerable<string> EnumerateEmptyDirectories(string path, int remainingDepth)
		{
			if (remainingDepth == 0) yield break;

			foreach (var dir in Directory.EnumerateDirectories(path))
			{
				if (Directory.EnumerateFiles(dir).Any()) continue;

				var subdirs = Directory.EnumerateDirectories(dir).ToList();

				foreach (var rec in EnumerateEmptyDirectories(dir, remainingDepth-1))
				{
					yield return rec;
					subdirs.Remove(rec);
				}

				if (subdirs.Count==0) yield return dir;
			}
		}

		public static string[] ReadAllLinesSafe(String path)
		{
			using (var csv = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (var sr = new StreamReader(csv))
				{
					List<string> file = new List<string>();
					while (!sr.EndOfStream)
					{
						file.Add(sr.ReadLine());
					}

					return file.ToArray();
				}
			}
		}

		public static void DeleteDirectoryWithRetry(string path)
		{
			for (int i = 0; i < 5; i++)
			{
				try
				{
					Directory.Delete(path);
					return;
				}
				catch (IOException)
				{
					Thread.Sleep(5);
				}
			}
			
			Directory.Delete(path); // Do it again and throw Exception if it fails
		}
	}
}
