using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
                service.Invoke("findFilesMissingFromDatabase", "non-existant path");
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
            List<string> paths = service.Invoke("findPathsInDatabase", String.Format(@"{0}\testdata", System.IO.Directory.GetCurrentDirectory())) as List<string>;
            Assert.AreEqual(393, paths.Count);
        }
    }
}
