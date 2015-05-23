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
        public void TestFilePathsOnFilesytem()
        {
            CleanerService rawService = new CleanerService();
            PrivateObject service = new PrivateObject(rawService);
            List<string> paths = service.Invoke("findPathsOnFilesystem", Path.Combine(Directory.GetCurrentDirectory(), "testdata")) as List<string>;
            Assert.AreEqual(400, paths.Count);
        }
    }
}
