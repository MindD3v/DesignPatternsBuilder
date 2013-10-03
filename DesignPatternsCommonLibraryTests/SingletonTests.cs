using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignPatternsCommonLibrary;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace DesignPatternsCommonLibraryTests
{
    public class SingletonTests
    {
        private DesignPattensFileManager _dpFileManager;
        public SingletonTests(DesignPattensFileManager dpFileManager)
        {
            _dpFileManager = dpFileManager;
        }
        public void SingletonBuilder(String type)
        {
            var classValues = new Dictionary<string, string>
                {
                    {"{NAMESPACE}", "BuiltDesignPatternsTest.SingletonTest"},
                    {"{CLASS_NAME}", "My"+type}
                };

            var designPatternBuilder = new DesignPatternBuilder(_dpFileManager);
            var classInformation = designPatternBuilder.BuildFromXml(type, classValues, null).First();

            _dpFileManager.CreateFile(classInformation.FileName, "TestDrops",
                                                              classInformation.Content);
            var fileExits = _dpFileManager.FileExistsInFolder(classInformation.FileName, "TestDrops");
            Assert.IsTrue(fileExits);
        }
    }
}
