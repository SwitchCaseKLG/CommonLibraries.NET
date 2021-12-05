﻿namespace SwitchCase.Core
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
                    case DateType.UPDATE: return File.GetLastAccessTime(path);
                    case DateType.NEWEST: return new[] { File.GetCreationTime(path), File.GetLastAccessTime(path) }.Max();
                    case DateType.OLDTEST: return new[] { File.GetCreationTime(path), File.GetLastAccessTime(path) }.Min();
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

            var slash = (absolutepath.IndexOf("/") > -1 ? "/" : "\\");

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

        public static void MoveContentsOfDirectory(string source, string target, bool renameExisting = false)
        {
            Directory.CreateDirectory(target);

            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(target, Path.GetFileName(file));
                if (File.Exists(dest))
                {
                    if (renameExisting)
                    {
                        int index = 1;
                        do
                        {
                            index++;
                            dest = Path.Combine(target, Path.GetFileNameWithoutExtension(file) + "_(" + index + ")" + Path.GetExtension(file));
                        } while (File.Exists(dest));

                        File.Move(file, dest);
                    }
                }
                else
                {
                    File.Move(file, dest);
                }
            }

            foreach (var dir in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(target, Path.GetFileName(dir));
                MoveContentsOfDirectory(dir, dest, renameExisting);
            }

            Directory.Delete(source, true);
        }

        public static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(this DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                CopyAll(diSourceSubDir, target.CreateSubdirectory(diSourceSubDir.Name));
            }
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
    }
}