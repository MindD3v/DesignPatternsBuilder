using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using Windows.Storage;

namespace DesignPatternsManagerW8
{
    public class DesignPatternsReader
    {
        private IDesignPattensFileManager _fileManager;
        public DesignPatternsReader(IDesignPattensFileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public async Task<IEnumerable<DesignPatternFile>> UpdateDesignPatterns(bool forceUpdate = false)
        {
            var fileExists = await _fileManager.FileExistsInFolder("DesignPatternsList.dsxml", await _fileManager.GetDesignPatternsTemplatesPath());

            if (!fileExists || forceUpdate)
            {
                return await UpdateDesignPatternsFile();
            }

            var readFileXml = await _fileManager.ReadFile("DesignPatternsList.dsxml", await _fileManager.GetDesignPatternsTemplatesPath());

            var designPatternsXml = XDocument.Parse(readFileXml);
            var designPatternsXmlCount = designPatternsXml.Descendants("DesignPattern").Count();
            var designPatternsFilesCount = await GetDesignPatternsFiles();

            if (designPatternsFilesCount.Count() != designPatternsXmlCount)
            {
                return await UpdateDesignPatternsFile();
            }

            var dpList = new List<DesignPatternFile>();
            var i = 0;
            foreach (var dp in designPatternsXml.Descendants("DesignPattern"))
            {
                dpList.Add(new DesignPatternFile
                {
                    Id = i,
                    DesignPatternName = dp.Descendants("Name").FirstOrDefault().Value,
                    DesignPatternType = dp.Descendants("type").FirstOrDefault().Value,
                    Description = dp.Descendants("Description").FirstOrDefault().Value.Trim(),
                    Path = dp.Descendants("Name").FirstOrDefault().Value.Replace(" ","") + ".xml"
                });
                i++;
            }


            return dpList;
        }
        public async Task<IEnumerable<DesignPatternParameter>> GetParametersFromDesignPattern(String patternPath)
        {
            var fileManager = new FileManager();
            var readFile = await fileManager.ReadFile(patternPath, await fileManager.GetDesignPatternsTemplatesPath());
            var designPatternXml = XDocument.Parse(readFile);

            var parameters = designPatternXml.Descendants("Parameter");

            var patternParameters = from p in parameters
                                    select new DesignPatternParameter
                                        {
                                            Name = p.Attribute("name").Value,
                                            Description = p.Attribute("description").Value,
                                            IsMultiple =
                                                p.Attribute("multiple") != null &&
                                                Boolean.Parse(p.Attribute("multiple").Value)
                                        };

            return patternParameters;
        }
        private async Task<IEnumerable<StorageFile>> GetDesignPatternsFiles()
        {
            var designPatternsTemplatesFiles = await _fileManager.GetFilesFromFolder(await _fileManager.GetDesignPatternsTemplatesPath(), new[] { ".xml" });

            return designPatternsTemplatesFiles;
        }
        private async Task<IEnumerable<DesignPatternFile>> UpdateDesignPatternsFile()
        {
            var designPatternFiles = new List<DesignPatternFile>();
            try
            {
                var designPatternsXml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("DesignPatterns")
                    );
                var files = await GetDesignPatternsFiles();
                var i = 0;
                foreach (var f in files)
                {
                    var readFile = await _fileManager.ReadFile(f.Name,await _fileManager.GetDesignPatternsTemplatesPath());
                    var doc = XDocument.Parse(readFile);
                    var designPattern = doc.Descendants("DesignPattern").FirstOrDefault();
                    var fileName = designPattern.Attribute("name").Value;
                    var type = designPattern.Attribute("type").Value;
                    var description = designPattern.Descendants("Description").FirstOrDefault();
                    var modifiedDate = (await f.GetBasicPropertiesAsync()).DateModified;

                    var xmlFile = new XElement("DesignPattern",
                                               new XElement("Name", fileName,
                                                            new XAttribute("modifiedDate", modifiedDate)),
                                               new XElement("Description", description.Value.Trim()),
                                               new XElement("type", type));
                    designPatternsXml.Element("DesignPatterns").Add(xmlFile);

                    var designPatternFile = new DesignPatternFile
                    {
                        Id = i,
                        Description = description.Value.Trim(),
                        DesignPatternName = fileName,
                        DesignPatternType = type,
                        Path = f.Name
                    };
                    designPatternFiles.Add(designPatternFile);
                    i++;
                }
                await _fileManager.CreateFile("DesignPatternsList.dsxml", await _fileManager.GetDesignPatternsTemplatesPath(),
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
