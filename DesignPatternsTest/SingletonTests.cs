using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesignPatterns;
using System.IO;
using System.Linq;
using DesignPatterns.DesignPatternsEnum;

namespace DesignPatternsTest
{
    [TestClass]
    public class SingletonTests
    {
        private const String DropPath = @"C:\Users\jhinojosa\Source\Repos\DesignPatterns\BuiltDesignPatternsTest";

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
        private void SingletonBuilder(String type)
        {
            var classValues = new Dictionary<string, string>
                {
                    {"{NAMESPACE}", "BuiltDesignPatternsTest.SingletonTest"},
                    {"{CLASS_NAME}", "My"+type}
                };

            var classInformation = DesignPatternBuilder.BuildFromXml(type, classValues, null).First();

            var fw = new FileWriter(DropPath + "\\SingletonTest\\" + classInformation.FileName, classInformation.Content);
            fw.WriteFile();

            Assert.IsTrue(File.Exists(DropPath + "\\SingletonTest\\" + classInformation.FileName));
        }

        
    }
}
