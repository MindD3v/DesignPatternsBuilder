using System;
using System.Collections.Generic;
using DesignPatternsCommonLibrary;
using DesignPatternsCommonLibraryTests;
using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class SingletonBuildingW8Tests
    {
        private SingletonTests _singletonTest;
        [TestInitialize]
        public void TestSetup()
        {
            _singletonTest = new SingletonTests(new DesignPattensFileManagerImplementation());
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
