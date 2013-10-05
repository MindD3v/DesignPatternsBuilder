using DesignPatternsCommonLibraryTests;
using DesignPatternsManagerW8;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace DesignPatternsManagerW8Tests
{
    [TestClass]
    public class DesignPatternUpdaterW8Tests
    {
        private DesignPatternsUpdaterTests _designPatternsUpdaterTests;
        [TestInitialize]
        public void TestSetup()
        {
            _designPatternsUpdaterTests = new DesignPatternsUpdaterTests(new DesignPattensFileManager());
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
