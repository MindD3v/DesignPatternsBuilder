using DesignPatternsCommonLibraryTests;
using DesignPatternsManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatternsTest
{
    [TestClass]
    public class DesignPatternsUpdaterWTests
    {
        private DesignPatternsUpdaterTests _designPatternsUpdaterTests;
        [TestInitialize]
        public void TestSetup()
        {
            _designPatternsUpdaterTests = new DesignPatternsUpdaterTests(new DesignPattensFileManagerImplementation());
        }
        [TestMethod]
        public void UpdateDesignPatternsTest()
        {
            _designPatternsUpdaterTests.UpdateDesignPatternsTest();
        }
        [TestMethod]
        public void CreateNewDesignPattern()
        {
            _designPatternsUpdaterTests.CreateNewDesignPattern();
        }
    }
}
