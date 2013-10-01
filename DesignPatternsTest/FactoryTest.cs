using System;
using System.Collections.Generic;
using System.IO;
using DesignPatternsManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatternsTest
{
    [TestClass]
    public class FactoryTest
    {
        private const String DropPath = @"C:\Users\jhinojosa\Source\Repos\DesignPatterns\BuiltDesignPatternsTest\";
        [TestMethod]
        public void ShapeExplicitFactory()
        {
            FactoryCreation("FactoryExplicit");
        }

        [TestMethod]
        public void ShapeFactoryHardcodedStrings()
        {
            FactoryCreation("FactoryHardcodedStrings");
        }

        private void FactoryCreation(String factoryType)
        {
            var parameters = new Dictionary<string, string>
                {
                    {"{NAMESPACE}", "BuiltDesignPatternsTest.FactoryTest.Shape"+factoryType},
                    {"{PARENT_OBJECT}", "Shape"}
                };
            var shapes = new Dictionary<String, List<String>>
                {
                    {
                        "{OBJECT}", new List<string>
                            {
                                "Circle",
                                "Square"
                            }
                    }
                };

            var files = DesignPatternBuilder.BuildFromXml(factoryType, parameters, shapes);

            foreach (var classInformation in files)
            {
                var fw = new FileWriter(DropPath + "\\FactoryTest\\Shape" + factoryType + "\\" + classInformation.FileName, classInformation.Content);
                fw.WriteFile();

                Assert.IsTrue(File.Exists(DropPath + "\\FactoryTest\\Shape" + factoryType + "\\" + classInformation.FileName));
            }
        }

    }
}
