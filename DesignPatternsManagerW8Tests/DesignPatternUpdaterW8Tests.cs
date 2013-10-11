using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using System;
using System.Xml.Linq;
using System.Linq;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class DesignPatternUpdaterW8Tests
    {
        private IDesignPattensFileManager _designPattensFileManager;
        private StorageFolder _designPatternsTemplatesPath;
        [TestInitialize]
        public void TestSetup()
        {
            _designPattensFileManager = new FileManager();
            _designPatternsTemplatesPath = _designPattensFileManager.GetDesignPatternsTemplatesPath().Result;
        }
        [TestMethod]
        public void UpdateDesignPatternsTest()
        {
            var files = _designPattensFileManager.GetFilesFromFolder(_designPatternsTemplatesPath, new[] { ".xml" }).Result;

            var updater = new DesignPatternsReader(_designPattensFileManager);
            var designPatternFiles = updater.UpdateDesignPatterns().Result;

            Assert.AreEqual(files.Count(), designPatternFiles.Count());
        }
        [TestMethod]
        public void CreateNewDesignPattern()
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("DesignPattern",
                    new XAttribute("name", "PatternTest"),
                    new XElement("Description", "Blablabla"),
                    new XAttribute("type", "Test"))
                );

            _designPattensFileManager.CreateFile("PatternTest.xml",
                                                 _designPatternsTemplatesPath, doc.ToString());

            UpdateDesignPatternsTest();

            _designPattensFileManager.DeleteFile("PatternTest.xml",
                                                 _designPatternsTemplatesPath);
            UpdateDesignPatternsTest();
        }
    }
}
