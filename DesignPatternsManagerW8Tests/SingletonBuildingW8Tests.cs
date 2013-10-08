using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class SingletonBuildingW8Tests
    {
        private IDesignPattensFileManager _dpFileManager;
        [TestInitialize]
        public void TestSetup()
        {
            _dpFileManager = new DesignPattensFileManager();
        }
        [TestMethod]
        public void SingletonCanonical()
        {
            SingletonBuilder("SingletonCanonical");
        }

        [TestMethod]
        public void SingletonStaticInitialization()
        {
            SingletonBuilder("SingletonStaticInitialization");
        }

        [TestMethod]
        public void SingletonMultithreaded()
        {
            SingletonBuilder("SingletonMultithreaded");
        }

        [TestMethod]
        public void SingletonLazy()
        {
            SingletonBuilder("SingletonLazy");
        }

        private async void SingletonBuilder(String type)
        {
            var classValues = new Dictionary<string, string>
                {
                    {"{NAMESPACE}", "BuiltDesignPatternsTest.SingletonTest"},
                    {"{CLASS_NAME}", "My"+type}
                };

            var designPatternBuilder = new DesignPatternBuilder(_dpFileManager);
            var classInformation = designPatternBuilder.BuildFromXml(type+".xml", classValues, null).Result.First();

            var folder = await _dpFileManager.GetApplicationStorageFolder("TestDrops");
            await _dpFileManager.CreateFile(classInformation.FileName, folder,
                                                              classInformation.Content);
            var fileExits = _dpFileManager.FileExistsInFolder(classInformation.FileName, folder).Result;
            Assert.IsTrue(fileExits);
        }
    }
}
