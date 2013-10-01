using System;
using BuiltDesignPatternsTest.FactoryTest.ShapeFactoryHardcodedStrings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuiltDesignPatternsTest.FactoryTest
{
    [TestClass]
    public class FactoryHardcodedStringsTests
    {
        [TestMethod]
        public void ShapeFactoryHardcodedStringsTest()
        {
            ShapeFactory myFactory = new ShapeFactory();
            IShape myCircle = myFactory.Make("Circle");
            IShape mySquare = myFactory.Make("Square");

            Assert.IsTrue(myCircle is Circle);
            Assert.IsTrue(mySquare is Square);

            Assert.IsFalse(myCircle is Square);
            Assert.IsFalse(mySquare is Circle);
        }
    }
}
