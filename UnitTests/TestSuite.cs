using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace CalibreCleaner
{
    [TestClass]
    public class CleanerServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(CleanerServiceException))]
        public void TestNoCalibreDatabasePresent()
        {
            try
            {
                CleanerService rawService = new CleanerService();
                PrivateObject service = new PrivateObject(rawService);
                // TODO: Switch to higher-level method once it's written
                service.Invoke("findPathsInDatabase", "non-existant path");
            }
            catch (CleanerServiceException e)
            {
                Assert.AreEqual("Could not open a \"metadata.db\" database file in the specified path", e.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestFindPathsInDatabase()
        {
            CleanerService rawService = new CleanerService();
            PrivateObject service = new PrivateObject(rawService);
            List<string> paths = service.Invoke("findPathsInDatabase", Path.Combine(Directory.GetCurrentDirectory(), "testdata")) as List<string>;
            Assert.AreEqual(393, paths.Count);
        }

        [TestMethod]
        public void TestAddGitignore()
        {
            string gitignoreText =
@"#This is just here to force Git to allow an empty directory to be committed.  This empty 
# directory structure is used by the unit test suite.
#
# The line below causes Git to ignore everyting in this directory except for the .gitignore 
# file itself.
# 
!.gitignore
";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "testdata");
            foreach (string authorDirectory in Directory.GetDirectories(path))
            {
                foreach (string bookDirectory in Directory.GetDirectories(Path.Combine(path, authorDirectory)))
                {
                    string filename = Path.Combine(path, authorDirectory, bookDirectory, ".gitignore");
                    Console.WriteLine(filename);
                    if (!File.Exists(filename))
                    {
                        using (StreamWriter writer = File.CreateText(filename))
                        {
                            writer.Write(gitignoreText);                          
                        }
                    }
                }
            }
        }
    }
}
