using System;
using System.Collections.Generic;
using DesignPatternsCommonLibraryTests;
using DesignPatternsManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace DesignPatternsTest
{
    [TestClass]
    public class SingletonBuildingTests
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
