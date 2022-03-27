using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchCase.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchCase.Core.Tests
{
    [TestClass()]
    public class FilesTests
    {
        [TestMethod()]
        public void CopyFileTest()
        {
            string file = "test.txt";
            string targetFile = "test_(2).txt";
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
            Directory.CreateDirectory("TEST/dir");
            File.WriteAllText(path, "test");
            Files.CopyFile(path, target, Files.DuplicateHandling.RENAME);
            Files.CopyFile(path, target, Files.DuplicateHandling.RENAME);
            Files.CopyFile(path, file, Files.DuplicateHandling.RENAME);
            Files.CopyFile(path, file, Files.DuplicateHandling.RENAME);
            Assert.IsTrue(File.Exists(target));
            Assert.IsTrue(File.Exists("TEST/dir/test_(2).txt"));
            Assert.IsTrue(File.Exists("test_(2).txt"));
        }
    }
}