using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DesignPatterns
{
    public class DesignPatternBuilder
    {
        public static String BuildFromXml(String patternName, Dictionary<string, string> parameters)
        {
            var designPatternTemplatesPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var doc = XDocument.Load(Path.GetDirectoryName(designPatternTemplatesPath) 
                + "\\DesignPatternsTemplates\\" + patternName + ".xml");

            var patternTemplate = (from d in doc.Descendants("Pattern")
                                   select d).FirstOrDefault();

            var replaceableParameters = (from p in doc.Descendants("Parameter")
                              select p).ToList();

            var patternTemplateReplaced = new StringBuilder(patternTemplate.Value);

            foreach (var parameter in replaceableParameters)
            {
                var parameterName = parameter.Attribute("name");
                var parameterValue = parameters.ContainsKey(parameterName.Value)
                                         ? parameters[parameterName.Value]
                                         : String.Empty;
                patternTemplateReplaced = patternTemplateReplaced.Replace(parameterName.Value, parameterValue);
            }

            return patternTemplateReplaced.ToString();
        }
    }
}
