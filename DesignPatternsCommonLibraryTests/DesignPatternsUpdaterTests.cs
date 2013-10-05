using System.Linq;
using System.Xml.Linq;
using DesignPatternsCommonLibrary;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace DesignPatternsCommonLibraryTests
{
    public class DesignPatternsUpdaterTests
    {
        private IDesignPattensFileManager _designPattensFileManager;
        public DesignPatternsUpdaterTests(IDesignPattensFileManager designPattensFileManager)
        {
            _designPattensFileManager = designPattensFileManager;
        }
        public async void UpdateDesignPatternsTest()
        {
            var files = _designPattensFileManager.GetFilesFromFolder(
                    _designPattensFileManager.GetDesignPatternsTemplatesPath(), new[] { ".xml" }).Result;

            var updater = new DesignPatternsUpdater(_designPattensFileManager);
            var designPatternFiles = updater.UpdateDesignPatterns().Result;

           Assert.AreEqual(files.Count(), designPatternFiles.Count());
        }
        public void CreateNewDesignPattern()
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("DesignPattern",
                    new XAttribute("name", "PatternTest"),
                    new XAttribute("type", "Test"))
                );

            _designPattensFileManager.CreateFile("PatternTest.xml",
                                                 _designPattensFileManager.GetDesignPatternsTemplatesPath(), doc.ToString());

            UpdateDesignPatternsTest();

            _designPattensFileManager.DeleteFile("PatternTest.xml",
                                                 _designPattensFileManager.GetDesignPatternsTemplatesPath());
            UpdateDesignPatternsTest();
        }
    }
    
}
