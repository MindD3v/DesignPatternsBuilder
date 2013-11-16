using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace DesignPatternsManagerW8
{
    public class DesignPatternFileParser
    {
        private FileManager _fileManager;

        public DesignPatternFileParser(FileManager fileManager)
        {
            _fileManager = fileManager;
        }
        public async Task<DesignPatternFile> Parse(StorageFile file)
        {
            var readFile = await _fileManager.ReadFile(file.Name, await _fileManager.GetDesignPatternsTemplatesPath());
            var doc = XDocument.Parse(readFile);
            var designPattern = doc.Descendants("DesignPattern").FirstOrDefault();
            if(designPattern == null)
                throw new Exception("Design Pattern node not found in file "+file.Name);

            var fileName = designPattern.Attribute("name").Value;
            var type = designPattern.Attribute("type").Value;
            var description = designPattern.Descendants("Description").FirstOrDefault().Value;
            var modifiedDate = (await file.GetBasicPropertiesAsync()).DateModified;

            var dpFile = new DesignPatternFile
                {
                    Description = description,
                    DesignPatternName = fileName,
                    DesignPatternType = type,
                    Path = file.Name,
                    ModifiedDate = modifiedDate
                };

            return dpFile;
        }
    }
}
