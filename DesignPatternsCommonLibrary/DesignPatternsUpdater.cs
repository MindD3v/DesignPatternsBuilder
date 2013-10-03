using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace DesignPatternsCommonLibrary
{
    public class DesignPatternsUpdater
    {
        private DesignPattensFileManager _fileManager;
        public DesignPatternsUpdater(DesignPattensFileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public List<DesignPatternFile> UpdateDesignPatterns()
        {
            var fileExists = _fileManager.FileExistsInFolder("DesignPatternsList.dsxml",
                                                            _fileManager.DesignPatternsTemplatesPath);

            if (!fileExists)
            {
                return UpdateDesignPatternsFile();
            }

            var readFileXml = _fileManager.ReadFile("DesignPatternsList.dsxml", _fileManager.DesignPatternsTemplatesPath);

            var designPatternsXml = XDocument.Parse(readFileXml);
            var designPatternsXmlCount = designPatternsXml.Descendants("DesignPattern").Count();
            var designPatternsFilesCount = GetDesignPatternsFiles();

            if (designPatternsFilesCount.Count() != designPatternsXmlCount)
            {
                return UpdateDesignPatternsFile();
            }

            return (from dp in designPatternsXml.Descendants("DesignPattern")
                    select new DesignPatternFile
                        {
                            DesignPatternName = dp.Value,
                            DesignPatternType = dp.Attribute("type").Value,
                            Path = _fileManager.DesignPatternsTemplatesPath + "\\" + dp.Value + ".xml"
                        }).ToList();
        }

        private IEnumerable<String> GetDesignPatternsFiles()
        {
            var designPatternsTemplatesFiles =
                _fileManager.GetFilesFromFolder(_fileManager.DesignPatternsTemplatesPath, new[] { ".xml" });
            return designPatternsTemplatesFiles.ToList();
        }
        private List<DesignPatternFile> UpdateDesignPatternsFile()
        {
            var designPatternFiles = new List<DesignPatternFile>();
            try
            {
                var designPatternsXml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("DesignPatterns")
                    );
                var files = GetDesignPatternsFiles();
                foreach (var f in files)
                {
                    var readFile = _fileManager.ReadFile(f, _fileManager.DesignPatternsTemplatesPath);
                    var doc = XDocument.Parse(readFile);
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
                _fileManager.CreateFile("DesignPatternsList.dsxml", _fileManager.DesignPatternsTemplatesPath,
                                        designPatternsXml.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message); 
            }
            

            return designPatternFiles;
        }
    }
}
