using System.IO;
using System.Linq;
using System.Xml.Linq;
using DesignPatternsManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatternsTest
{
    [TestClass]
    public class DesignPatternsUpdaterTests
    {
        private string _path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                + "\\DesignPatternsTemplates";
        [TestMethod]
        public void XmlBuild()
        {
            var files = Directory.GetFiles(_path, "*.xml", SearchOption.AllDirectories).ToList();

            var updater = new DesignPatternsUpdater(_path);
            var designPatternFiles = updater.UpdateDesignPatterns();

            Assert.AreEqual(files.Count(),designPatternFiles.Count());
        }

        [TestMethod]
        public void CreateNewDesignPattern()
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("DesignPattern",
                    new XAttribute("name", "PatternTest"),
                    new XAttribute("type", "Test"))
                );
            doc.Save(_path + "\\PatternTest.xml");
            XmlBuild();

            File.Delete(_path + "\\PatternTest.xml");
            XmlBuild();
        }
    }
}
