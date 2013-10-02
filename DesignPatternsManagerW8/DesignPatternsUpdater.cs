using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using FileManagerLibrary;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;

namespace DesignPatternsManagerW8
{
    public class DesignPatternsUpdater
    {
        private StorageFolder _applicationFolder;
        private StorageFolder _libraryFolder;
        private StorageFolder _designPatternsTemplatesFolder;

        private IFileManager FileManager;
        public DesignPatternsUpdater(IFileManager fileManager)
        {
            GetPaths();
            FileManager = fileManager;
        }
        public async Task<List<DesignPatternFile>> UpdateDesignPatterns()
        {
            var fileExists = FileManager.FileExistsInFolder("DesignPatternsList.dsxml",
                                                            "DesignPatternsManagerW8\\DesignPatternsTemplates\\");

            if (!fileExists)
            {
                return await UpdateDesignPatternsFile();
            }

            var designPatternsXml = XDocument.Load(_designPatternsTemplatesFolder + "\\DesignPatternsList.dsxml");
            var designPatternsXmlCount = designPatternsXml.Descendants("DesignPattern").Count();
            var designPatternsFilesCount = GetDesignPatternsFiles();

            if (designPatternsFilesCount.Count() != designPatternsXmlCount)
            {
                return await UpdateDesignPatternsFile();
            }

            return (from dp in designPatternsXml.Descendants("DesignPattern")
                    select new DesignPatternFile
                        {
                            DesignPatternName = dp.Value,
                            DesignPatternType = dp.Attribute("type").Value,
                            Path = _designPatternsTemplatesFolder + "\\" + dp.Value + ".xml"
                        }).ToList();
        }
        private async void GetPaths()
        {
            _applicationFolder = Package.Current.InstalledLocation;
            _libraryFolder = await _applicationFolder.GetFolderAsync("DesignPatternsManagerW8");
            _designPatternsTemplatesFolder = await _libraryFolder.GetFolderAsync("DesignPatternsTemplates");
        }
        private IEnumerable<String> GetDesignPatternsFiles()
        {
            var designPatternsTemplatesFiles =
                FileManager.GetFilesFromFolder("DesignPatternsManagerW8\\DesignPatternsTemplates\\",new[] {".xml"});
            return designPatternsTemplatesFiles.ToList();
        }
        private async Task<List<DesignPatternFile>> UpdateDesignPatternsFile()
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

                var designPatternsList = await _designPatternsTemplatesFolder.CreateFileAsync("DesignPatternsList.dsxml", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(designPatternsList, designPatternsXml.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message); 
            }
            

            return designPatternFiles;
        }
    }
}
