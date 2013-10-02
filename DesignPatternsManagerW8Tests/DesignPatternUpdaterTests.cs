using System;
using System.Linq;
using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class DesignPatternUpdaterTests
    {
        public StorageFolder _applicationFolder;
        public StorageFolder _libraryFolder;
        public StorageFolder _designPatternsTemplatesFolder;

        [TestInitialize]
        public void TestSetup()
        {
            InitializePaths();
        }

        [TestMethod]
        public void XmlBuild()
        {
            UpdateDesignPatternsTest();
        }

        private async void UpdateDesignPatternsTest()
        {
            var files = (await _designPatternsTemplatesFolder.GetFilesAsync(CommonFileQuery.OrderByName)).ToList();

            var updater = new DesignPatternsUpdater(new FileManager());
            var designPatternFiles = (await updater.UpdateDesignPatterns());

            Assert.AreEqual(files.Count(), designPatternFiles.Count());
        }
        private async void InitializePaths()
        {
            _applicationFolder = Package.Current.InstalledLocation;
            _libraryFolder = await _applicationFolder.GetFolderAsync("DesignPatternsManagerW8");
            _designPatternsTemplatesFolder = await _libraryFolder.GetFolderAsync("DesignPatternsTemplates");
        }
    }
}
