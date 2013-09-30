using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesignPatterns;
using System.IO;
using DesignPatterns.DesignPatternsEnum;

namespace DesignPatternsTest
{
    [TestClass]
    public class SingletonTests
    {
        [TestMethod]
        public void SingletonCanonical()
        {
            String singleton = SingletonBuilder.BuildSingleton("MySingletonCanonical", SingletonType.Canonical);

            FileWriter fw = new FileWriter(@"C:\Users\JuanLorenzo\Desktop\MySingletonCanonical.cs", singleton);
            fw.WriteFile();

            Assert.IsTrue(File.Exists(@"C:\Users\JuanLorenzo\Desktop\SingletonCanonical.cs"));
        }
        [TestMethod]
        public void SingletonStaticInitialization()
        {
            String singleton = SingletonBuilder.BuildSingleton("MySingletonStaticInitialization", SingletonType.StaticInitialization);

            FileWriter fw = new FileWriter(@"C:\Users\JuanLorenzo\Desktop\MySingletonStaticInitialization.cs", singleton);
            fw.WriteFile();

            Assert.IsTrue(File.Exists(@"C:\Users\JuanLorenzo\Desktop\test.cs"));
        }

        [TestMethod]
        public void SingletonCanonicalXML()
        {
            String singleton = SingletonBuilder.BuildSingletonFromXml("MySingletonStaticInitialization", "SingletonCanonical");

            FileWriter fw = new FileWriter(@"C:\Users\JuanLorenzo\Desktop\MySingletonStaticInitialization.cs", singleton);
            fw.WriteFile();

            Assert.IsTrue(File.Exists(@"C:\Users\JuanLorenzo\Desktop\test.cs"));
        }
    }
}
