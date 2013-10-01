using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace DesignPatternsManager
{
    public class DesignPatternsUpdater
    {
        private String _path;
        public DesignPatternsUpdater(String path)
        {
            _path = path;
        }
        public List<DesignPatternFile> UpdateDesignPatterns()
        {
            if (!File.Exists(_path + "\\DesignPatternsList.dsxml"))
            {
                return UpdateDesignPatternsFile();
            }


            var designPatternsXml = XDocument.Load(_path + "\\DesignPatternsList.dsxml");
            var designPatternsXmlCount = designPatternsXml.Descendants("DesignPattern").Count();
            var designPatternsFilesCount = GetDesignPatternsFiles().Count();

            if (designPatternsFilesCount != designPatternsXmlCount)
            {
                return UpdateDesignPatternsFile();
            }


            var designPatternFiles = new List<DesignPatternFile>();

            return (from dp in designPatternsXml.Descendants("DesignPattern")
                    select new DesignPatternFile
                        {
                            DesignPatternName = dp.Value,
                            DesignPatternType = dp.Attribute("type").Value,
                            Path = _path + "\\" + dp.Value + ".xml"
                        }).ToList();
        }
        private IEnumerable<string> GetDesignPatternsFiles()
        {
            return Directory.GetFiles(_path, "*.xml", SearchOption.AllDirectories).ToList();
        }
        private List<DesignPatternFile> UpdateDesignPatternsFile()
        {
            var designPatternFiles = new List<DesignPatternFile>();
            var designPatternsXml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("DesignPatterns")
                    );
            var files = GetDesignPatternsFiles();
            foreach (var f in files)
            {
                var doc = XDocument.Load(f);
                var designPattern = doc.Descendants("DesignPattern").FirstOrDefault();
                var fileName = designPattern.Attribute("name").Value;
                var type = designPattern.Attribute("type").Value;
                var xmlFile = new XElement("DesignPattern", fileName, new XAttribute("type", type));
                designPatternsXml.Element("DesignPatterns").Add(xmlFile);

                var designPatternFile = new DesignPatternFile
                    {
                        DesignPatternName = fileName,
                        DesignPatternType = type,
                        Path = f
                    };
                designPatternFiles.Add(designPatternFile);
            }
            designPatternsXml.Save(_path + "\\DesignPatternsList.dsxml");

            return designPatternFiles;
        }
    }
}
