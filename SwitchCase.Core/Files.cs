using System.IO;

namespace SwitchCase.Core
{
    public static class Files
    {
        public static IEnumerable<string> EnumerateDirectories(string path)
        {
            return Directory.Exists(path) ? Directory.EnumerateDirectories(path) : Enumerable.Empty<string>();
        }

        public enum DateType
        {
            CREATE,
            UPDATE,
            ACCESS,
            OLDTEST,
            NEWEST
        }

        public static DateTime GetDate(string path, DateType dateType = DateType.UPDATE, DateTime? defaultValue = null)
        {
            if (File.Exists(path))
            {
                switch (dateType)
                {
                    case DateType.CREATE: return File.GetCreationTime(path);
                    case DateType.ACCESS: return File.GetLastAccessTime(path);
                    case DateType.UPDATE: return File.GetLastWriteTime(path);
                    case DateType.NEWEST: return new[] { File.GetCreationTime(path), File.GetLastAccessTime(path), File.GetLastWriteTime(path) }.Max();
                    case DateType.OLDTEST: return new[] { File.GetCreationTime(path), File.GetLastAccessTime(path), File.GetLastWriteTime(path) }.Min();
                    default: break;
                }
            }
            return defaultValue ?? DateTime.Now;
        }

        public static string ShrinkName(string name, int limit, string delimiter = "… ")
        {
            //no path provided
            if (string.IsNullOrEmpty(name))
            {
                return "";
            }
            string ext = Path.GetExtension(name);
            int namelen = name.Length;
            int delimlen = delimiter.Length;
            int extlen = ext.Length;

            //less than the minimum amt
            if (limit < (delimlen + extlen + 1))
            {
                return "";
            }

            //fullpath
            if (limit >= namelen)
            {
                return name;
            }

            return name.Substring(0, limit - delimlen - extlen) + delimiter + ext;
        }

        public static string ShrinkPath(string absolutepath, int limit, string delimiter = "…")
        {
            //no path provided
            if (string.IsNullOrEmpty(absolutepath))
            {
                return "";
            }

            var name = Path.GetFileName(absolutepath);
            int namelen = name.Length;
            int pathlen = absolutepath.Length;
            var dir = absolutepath.Substring(0, pathlen - namelen);

            int delimlen = delimiter.Length;
            int idealminlen = namelen + delimlen;

            var slash = absolutepath.IndexOf("/") > -1 ? "/" : "\\";

            //less than the minimum amt
            if (limit < ((2 * delimlen) + 1))
            {
                return "";
            }

            //fullpath
            if (limit >= pathlen)
            {
                return absolutepath;
            }

            //file name condensing
            if (limit < idealminlen)
            {
                return delimiter + name.Substring(0, (limit - (2 * delimlen))) + delimiter;
            }

            //whole name only, no folder structure shown
            if (limit == idealminlen)
            {
                return delimiter + name;
            }

            return dir.Substring(0, (limit - (idealminlen + 1))) + delimiter + slash + name;
        }

        public enum DuplicateHandling
        {
            OVERWRITE,
            SKIP,
            RENAME
        }

        public enum FolderDuplicateHandling
        {
            MERGE,
            SKIP,
            RENAME
        }

        public static string GetRenamedFileName(string path)
        {
            string newPath = path;
            if (File.Exists(path))
            {
                int index = 1;
                do
                {
                    index++;
                    newPath = Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, Path.GetFileNameWithoutExtension(path) + "_(" + index + ")" + Path.GetExtension(path));
                } while (File.Exists(newPath));
            }
            return newPath;
        }

        public static string GetRenamedFolderName(string path)
        {
            string newPath = path;
            if (Directory.Exists(path))
            {
                int index = 1;
                do
                {
                    index++;
                    newPath = path + "_(" + index + ")";
                } while (File.Exists(newPath));
            }
            return newPath;
        }

        private static void CopyMoveFile(string source, string target, DuplicateHandling duplicateHandling, bool move)
        {
            if (!File.Exists(source)) return;

            if (File.Exists(target))
            {
                switch (duplicateHandling)
                {
                    case DuplicateHandling.OVERWRITE: CopyMoveFile(source, target, move); break;
                    case DuplicateHandling.RENAME:
                        {
                            CopyMoveFile(source, GetRenamedFileName(target), move);
                            break;
                        }
                    default: break;
                }
            }
            else
            {
                CopyMoveFile(source, target, move);
            }
        }

        private static void CopyMoveFile(string source, string target, bool move)
        {
            if (move)
            {
                File.Move(source, target, true);
            }
            else
            {
                File.Copy(source, target, true);
            }
        }

        private static void CreateParentDirectory(string path)
        {
            string? dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void CopyFile(string source, string target, DuplicateHandling duplicateHandling = DuplicateHandling.OVERWRITE)
        {
            CreateParentDirectory(target);
            CopyMoveFile(source, target, duplicateHandling, false);
        }

        public static void MoveFile(string source, string target, DuplicateHandling duplicateHandling = DuplicateHandling.OVERWRITE)
        {
            CreateParentDirectory(target);
            CopyMoveFile(source, target, duplicateHandling, true);
        }

        private static void CopyMoveDirectory(string source, string target, DuplicateHandling duplicateHandling, FolderDuplicateHandling folderDuplicateHandling, bool move)
        {
            if (!Directory.Exists(source)) return;

            if (Directory.Exists(target))
            {
                switch (folderDuplicateHandling)
                {
                    case FolderDuplicateHandling.SKIP: return;
                    case FolderDuplicateHandling.RENAME:
                        {
                            target = GetRenamedFileName(target);
                            break;
                        }
                    default: break;
                }
            }

            Directory.CreateDirectory(target);

            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(target, Path.GetFileName(file));
                CopyMoveFile(file, dest, duplicateHandling, move);
            }

            foreach (var dir in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(target, Path.GetFileName(dir));
                CopyMoveDirectory(dir, dest, duplicateHandling, folderDuplicateHandling, move);
            }

            if (move) Directory.Delete(source);
        }

        public static void CopyDirectory(string source, string target, DuplicateHandling duplicateHandling = DuplicateHandling.OVERWRITE, FolderDuplicateHandling folderDuplicateHandling = FolderDuplicateHandling.MERGE)
        {
            CopyMoveDirectory(source, target, duplicateHandling, folderDuplicateHandling, false);
        }

        public static void MoveDirectory(string source, string target, DuplicateHandling duplicateHandling = DuplicateHandling.OVERWRITE, FolderDuplicateHandling folderDuplicateHandling = FolderDuplicateHandling.MERGE)
        {
            CopyMoveDirectory(source, target, duplicateHandling, folderDuplicateHandling, true);
        }

        public static void SetToReadOnly(this FileInfo fInfo)
        {
            fInfo.Attributes |= FileAttributes.ReadOnly;
        }

        public static void SetToNormal(this FileInfo fInfo)
        {
            fInfo.Attributes = FileAttributes.Normal;
        }

        public static void SetToReadOnly(this DirectoryInfo dInfo)
        {
            // Set Directory attribute
            dInfo.Attributes |= FileAttributes.ReadOnly;

            foreach (FileInfo file in dInfo.GetFiles())
            {
                file.SetToReadOnly();
            }

            // recurse all of the subdirectories
            foreach (DirectoryInfo subDir in dInfo.GetDirectories())
            {
                subDir.SetToReadOnly();
            }
        }

        public static void SetToNormal(this DirectoryInfo dInfo)
        {
            // Set Directory attribute
            dInfo.Attributes = FileAttributes.Normal;

            foreach (FileInfo file in dInfo.GetFiles())
            {
                file.SetToNormal();
            }

            // recurse all of the subdirectories
            foreach (DirectoryInfo subDir in dInfo.GetDirectories())
            {
                subDir.SetToNormal();
            }
        }

        public static void DeleteIfExist(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static string ReplaceInvalidChars(string filename, string replacement = "_")
        {
            return string.Join(replacement, filename.Split(Path.GetInvalidFileNameChars()));
        }

        public static bool IsFileLocked(this FileInfo file)
        {
            try
            {
                using FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                stream.Close();
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
