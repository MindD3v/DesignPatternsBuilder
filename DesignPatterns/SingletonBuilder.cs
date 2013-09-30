using DesignPatterns.DesignPatternsEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DesignPatterns
{
    public class SingletonBuilder
    {
        
        private const String Canonical =
        @"using System;

public class {CLASS_NAME}
{
    private static {CLASS_NAME} instance;

    private {CLASS_NAME}() {}

    public static {CLASS_NAME} Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new {CLASS_NAME}();
            }
            return instance;
        }
    }
}";
        private const String StaticInitialization =
            @"using System;
public sealed class {CLASS_NAME}
{
   private static readonly {CLASS_NAME} instance = new {CLASS_NAME}();
   
   private {CLASS_NAME}(){}

   public static {CLASS_NAME} Instance
   {
      get 
      {
         return instance; 
      }
   }
}";
        public static String BuildSingleton(String className, SingletonType type)
        {
            switch (type)
            {
                case SingletonType.Canonical:
                    return new StringBuilder(Canonical).Replace("{CLASS_NAME}", className).ToString();
                case SingletonType.StaticInitialization:
                    return new StringBuilder(StaticInitialization).Replace("{CLASS_NAME}", className).ToString();
                case SingletonType.Multithreaded:
                case SingletonType.Lazy:
                default:
                    return String.Empty;
                    
            }
        }
        public static String BuildSingletonFromXml(String className, String singletonType)
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            XDocument doc = XDocument.Load(Path.GetDirectoryName(path) + "\\DesignPatternsTemplates\\" + singletonType + ".dp");

            XElement patternTemplate = (from d in doc.Descendants("Pattern")
                               select d).FirstOrDefault() ;

            String pattern = patternTemplate.Value.Replace("{CLASS_NAME}", className);

            return pattern.ToString();
        }
    }
}
