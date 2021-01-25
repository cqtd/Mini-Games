using System.IO;

namespace CQ.Editor
{
	public static class FileManagement
	{
		static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, string blacklist = null) {
			foreach (DirectoryInfo dir in source.GetDirectories())
			{
				if (!string.IsNullOrEmpty(blacklist) && dir.FullName.Contains(blacklist))
					continue;
				
				CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
			}
			
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name), true);
		}
	}
}
