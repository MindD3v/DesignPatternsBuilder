using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class FactoryBuildingW8Tests
    {
        private IDesignPattensFileManager _dpFileManager;
        [TestInitialize]
        public void TestSetup()
        {
            _dpFileManager = new FileManager();
        }

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
        private async void FactoryCreation(String factoryType)
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

            var designPatternBuilder = new DesignPatternBuilder(_dpFileManager);

            var files = designPatternBuilder.BuildFromXml(factoryType+".xml", parameters, shapes).Result;
            var folder = await _dpFileManager.GetApplicationStorageFolder("TestDrops");
            foreach (var classInformation in files)
            {
                await _dpFileManager.CreateFile(classInformation.FileName, folder,
                                                               classInformation.Content);
                var fileExits = _dpFileManager.FileExistsInFolder(classInformation.FileName, folder).Result;
                Assert.IsTrue(fileExits);
            }
        }
    }
}
