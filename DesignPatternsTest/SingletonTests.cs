using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesignPatterns;
using System.IO;
using DesignPatterns.DesignPatternsEnum;

namespace DesignPatternsTest
{
    [TestClass]
    public class SingletonTests
    {
        private const String DropPath = @"C:\Users\jhinojosa\Desktop";

        [TestMethod]
        public void SingletonCanonical()
        {
            var classValues = new Dictionary<string, string> {{"{CLASS_NAME}", "MySingletonCanonical"}};

            var singleton = DesignPatternBuilder.BuildFromXml("SingletonCanonical", classValues);

            var fw = new FileWriter(DropPath + @"\MySingletonCanonical.cs", singleton);
            fw.WriteFile();

            Assert.IsTrue(File.Exists(DropPath + @"\MySingletonCanonical.cs"));
        }

        [TestMethod]
        public void SingletonStaticInitialization()
        {
            var classValues = new Dictionary<string, string> { { "{CLASS_NAME}", "MySingletonStaticInitialization" } };

            var singleton = DesignPatternBuilder.BuildFromXml("SingletonStaticInitialization", classValues);

            var fw = new FileWriter(DropPath + @"\MySingletonStaticInitialization.cs", singleton);
            fw.WriteFile();

            Assert.IsTrue(File.Exists(DropPath + @"\MySingletonStaticInitialization.cs"));
        }
    }
}
