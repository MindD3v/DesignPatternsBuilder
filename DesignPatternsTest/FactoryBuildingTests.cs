using DesignPatternsCommonLibraryTests;
using DesignPatternsManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatternsTest
{
    [TestClass]
    public class FactoryBuildingTests
    {
        private FactoryTests _factoryTests;
        [TestInitialize]
        public void TestSetup()
        {
            _factoryTests = new FactoryTests(new DesignPattensFileManagerImplementation());
        }

        [TestMethod]
        public void ShapeExplicitFactory()
        {
            _factoryTests.FactoryCreation("FactoryExplicit");
        }

        [TestMethod]
        public void ShapeFactoryHardcodedStrings()
        {
            _factoryTests.FactoryCreation("FactoryHardcodedStrings");
        }
    }
}
