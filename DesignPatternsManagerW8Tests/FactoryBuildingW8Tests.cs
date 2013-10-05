using DesignPatternsCommonLibraryTests;
using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class FactoryBuildingW8Tests
    {
        private FactoryTests _factoryTests;
        [TestInitialize]
        public void TestSetup()
        {
            _factoryTests = new FactoryTests(new DesignPattensFileManager());
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
