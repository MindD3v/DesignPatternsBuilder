using BuiltDesignPatternsTest.FactoryTest.ShapeFactoryExplicit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuiltDesignPatternsTest.FactoryTest
{
    [TestClass]
    public class FactoryExplicitTests
    {
        [TestMethod]
        public void ShapeFactoryExplicitTest()
        {
            ShapeFactory myFactory = new ShapeFactory();
            IShape myCircle = myFactory.MakeCircle();
            IShape mySquare = myFactory.MakeSquare();

            Assert.IsTrue(myCircle is Circle);
            Assert.IsTrue(mySquare is Square);

            Assert.IsFalse(myCircle is Square);
            Assert.IsFalse(mySquare is Circle);
        }
    }
}
