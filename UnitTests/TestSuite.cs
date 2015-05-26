using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CalibreCleaner
{
    [TestClass]
    public class CleanerServiceTests
    {

        readonly string TestdataPath = Path.Combine(Directory.GetCurrentDirectory(), "testdata");

        [TestMethod]
        [ExpectedException(typeof(CleanerServiceException))]
        public void TestNoCalibreDatabasePresent()
        {
            try
            {
                List<BookMetadata> booksMissingInDatabase, booksMissingOnFilesystem;
                CleanerService.findMissingBooks("non-existant-path", out booksMissingInDatabase, out  booksMissingOnFilesystem);
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
            PrivateType service = new PrivateType(typeof(CleanerService));
            List<string> paths = service.InvokeStatic("findPathsInDatabase", TestdataPath) as List<string>;
            Assert.AreEqual(393, paths.Count);
        }

        [TestMethod]
        public void TestFilePathsOnFilesytem()
        {
            PrivateType service = new PrivateType(typeof(CleanerService));
            List<string> paths = service.InvokeStatic("findPathsOnFilesystem", TestdataPath) as List<string>;
            Assert.AreEqual(400, paths.Count);
        }

        [TestMethod]
        public void TestFindMissingBooks()
        {
            List<BookMetadata> booksMissingInDatabase, booksMissingOnFilesystem;
            CleanerService.findMissingBooks(TestdataPath, out booksMissingInDatabase, out booksMissingOnFilesystem);
            Assert.AreEqual(12, booksMissingInDatabase.Count);
            Assert.AreEqual(5, booksMissingOnFilesystem.Count);
        }
    }
}
