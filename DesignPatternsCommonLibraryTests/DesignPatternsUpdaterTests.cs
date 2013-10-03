using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DesignPatternsCommonLibrary;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace DesignPatternsCommonLibraryTests
{
    public class DesignPatternsUpdaterTests
    {
        private DesignPattensFileManager _designPattensFileManager;
        public DesignPatternsUpdaterTests(DesignPattensFileManager designPattensFileManager)
        {
            _designPattensFileManager = designPattensFileManager;
        }
        public void UpdateDesignPatternsTest()
        {
            var files =
                _designPattensFileManager.GetFilesFromFolder(
                    _designPattensFileManager.DesignPatternsTemplatesPath, new[] { ".xml" });

            var updater = new DesignPatternsUpdater(_designPattensFileManager);
            var designPatternFiles = updater.UpdateDesignPatterns();

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
                                                 _designPattensFileManager.DesignPatternsTemplatesPath, doc.ToString());

            UpdateDesignPatternsTest();

            _designPattensFileManager.DeleteFile("PatternTest.xml",
                                                 _designPattensFileManager.DesignPatternsTemplatesPath);
            UpdateDesignPatternsTest();
        }
    }
    
}
