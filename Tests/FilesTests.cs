using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace SwitchCase.Core.Tests
{
    [TestClass()]
    [DoNotParallelize]
    public class FilesTests
    {
        [TestMethod()]
        public void CopyFileTest()
        {
            string file = "test.txt";
            string targetFile = "test_(2).txt";
            string targetFile2 = "test_(3).txt";
            string root = "TEST";
            string path = "TEST/test_in.txt";
            string target = "TEST/dir/test.txt";
            if (Directory.Exists(root))
            {
                Directory.Delete(root, true);
            }
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }
            if (File.Exists(targetFile2))
            {
                File.Delete(targetFile2);
            }
            Directory.CreateDirectory("TEST/dir");
            File.WriteAllText(path, "test");
            Files.CopyFile(path, target, Files.DuplicateHandling.RENAME);
            Files.CopyFile(path, target, Files.DuplicateHandling.RENAME);
            Files.CopyFile(path, file, Files.DuplicateHandling.RENAME);
            Files.CopyFile(path, file, Files.DuplicateHandling.RENAME);
            Files.CopyFile(path, file, Files.DuplicateHandling.RENAME);
            Assert.IsTrue(File.Exists(target));
            Assert.IsTrue(File.Exists("TEST/dir/test_(2).txt"));
            Assert.IsTrue(File.Exists(targetFile));
            Assert.IsTrue(File.Exists(targetFile2));
        }

        [TestMethod()]
        public void CopyDirectoryTest()
        {
            string root = "DIRTEST";
            string source = "DIRTEST/INDIR";
            string sourceFile = "DIRTEST/INDIR/test.txt";
            string target = "DIRTEST/OUTDIR";

            if (Directory.Exists(root))
            {
                Directory.Delete(root, true);
            }

            Directory.CreateDirectory(source);
            File.WriteAllText(sourceFile, "test");
            Files.CopyDirectory(source, target, Files.DuplicateHandling.RENAME);
            Files.CopyDirectory(source, target, Files.DuplicateHandling.RENAME);
            Files.MoveDirectory(source, target, Files.DuplicateHandling.RENAME);

            Assert.IsTrue(File.Exists("DIRTEST/OUTDIR/test.txt"));
            Assert.IsTrue(File.Exists("DIRTEST/OUTDIR/test_(2).txt"));
            Assert.IsTrue(File.Exists("DIRTEST/OUTDIR/test_(3).txt"));
            Assert.IsFalse(Directory.Exists(source));
        }
    }
}