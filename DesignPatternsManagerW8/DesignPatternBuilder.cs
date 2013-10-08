using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DesignPatternsManagerW8
{
    public class DesignPatternBuilder
    {
        private IDesignPattensFileManager _designPattensFileManager;
        public DesignPatternBuilder(IDesignPattensFileManager designPattensFileManager)
        {
            _designPattensFileManager = designPattensFileManager;
        }
        public async Task<IEnumerable<ClassInformation>> BuildFromXml(String patternPath, Dictionary<string, string> parameters, Dictionary<string,List<String>> multipleObjects)
        {
            var files = new List<ClassInformation>();

            var readDesignPattern = await _designPattensFileManager.ReadFile(patternPath, await _designPattensFileManager.GetDesignPatternsTemplatesPath());

            var doc = XDocument.Parse(readDesignPattern);

            foreach (var f in doc.Descendants("File"))
            {
                var type = f.Attribute("type");
                if (type == null)
                {
                    files.Add(CreateFile(f, parameters, multipleObjects));
                }
                else if (type != null && type.Value == "Multiple")
                {
                    var bind = f.Attribute("bind").Value;
                    foreach (var obj in multipleObjects[bind])
                    {
                        files.Add(CreateFile(f, parameters, bind, obj));
                    }
                }
            }
            return files;
        }
        private ClassInformation CreateFile(XElement f, Dictionary<string, string> parameters, Dictionary<string, List<String>> multipleObjects)
        {
            var classInformation = new ClassInformation();

            var fileName = new StringBuilder(f.Descendants("Name").FirstOrDefault().Value);
            var extension = f.Descendants("Extension").FirstOrDefault().Value;
            classInformation.FileName = ReplaceParameters(fileName, parameters).ToString().Trim() + extension.Trim();

            var implementation = f.Descendants("Implementation").FirstOrDefault();
            var multiTemplate = f.Descendants("MultiTemplate").FirstOrDefault();

            var classFile = new StringBuilder(implementation.Value);

            if (multiTemplate != null)
            {
                var multiTemplateValue = multiTemplate.Value;
                var multiTemplateName = multiTemplate.Attribute("name").Value;
                var multiTemplateBind = multiTemplate.Attribute("bind").Value;
                var multipleObjectList = multipleObjects[multiTemplateBind];
                var templateBuilder = new StringBuilder();
                foreach (var obj in multipleObjectList)
                {
                    templateBuilder = templateBuilder.Append(multiTemplateValue).Replace(multiTemplateBind, obj);
                }

                classFile = classFile.Replace(multiTemplateName, templateBuilder.ToString());
            }
            classFile = ReplaceParameters(classFile, parameters);
            classInformation.Content = classFile.ToString();
            return classInformation;
        }
        private ClassInformation CreateFile(XElement f, Dictionary<string, string> parameters, String bindedObject,
                                  String bindedObjectValue)
        {
            var classInformation = new ClassInformation();

            var fileName = new StringBuilder(f.Descendants("Name").FirstOrDefault().Value);
            var extension = f.Descendants("Extension").FirstOrDefault().Value;
            classInformation.FileName = ReplaceParameters(fileName, parameters).Replace(bindedObject,bindedObjectValue).ToString().Trim() + extension.Trim();

            var implementation = f.Descendants("Implementation").FirstOrDefault();

            var classFile = new StringBuilder(implementation.Value);

            classFile = classFile.Replace(bindedObject, bindedObjectValue);
            
            classFile = ReplaceParameters(classFile, parameters);
            classInformation.Content = classFile.ToString();

            return classInformation;
        }
        private StringBuilder ReplaceParameters(StringBuilder replaceableStringBuilder, Dictionary<string, string> parameters)
        {

            foreach (var parameter in parameters)
            {
                replaceableStringBuilder = replaceableStringBuilder.Replace(parameter.Key, parameter.Value);
            }
            return replaceableStringBuilder;
        }
    }
}
