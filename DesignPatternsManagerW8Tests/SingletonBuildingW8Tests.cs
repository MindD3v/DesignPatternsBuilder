using DesignPatternsCommonLibraryTests;
using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class SingletonBuildingW8Tests
    {
        private SingletonTests _singletonTest;
        [TestInitialize]
        public void TestSetup()
        {
            _singletonTest = new SingletonTests(new DesignPattensFileManager());
        }
        [TestMethod]
        public void SingletonCanonical()
        {
            _singletonTest.SingletonBuilder("SingletonCanonical");
        }

        [TestMethod]
        public void SingletonStaticInitialization()
        {
            _singletonTest.SingletonBuilder("SingletonStaticInitialization");
        }

        [TestMethod]
        public void SingletonMultithreaded()
        {
            _singletonTest.SingletonBuilder("SingletonMultithreaded");
        }

        [TestMethod]
        public void SingletonLazy()
        {
            _singletonTest.SingletonBuilder("SingletonLazy");
        }
    }
}
