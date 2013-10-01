using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuiltDesignPatternsTest.SingletonTest
{
    [TestClass]
    public class SingletonTests
    {
        [TestMethod]
        public void MySingletonCanonicalTest()
        {
            MySingletonCanonical s1 = MySingletonCanonical.Instance;
            MySingletonCanonical s2 = MySingletonCanonical.Instance;

            Assert.AreSame(s1,s2);
        }

        [TestMethod]
        public void MySingletonCanonicalNoPublicConstructorsTest()
        {
            var singleton = typeof (MySingletonCanonical);
            var ctrs = singleton.GetConstructors();
            var hasPublicConstructor = false;
            foreach (var constructorInfo in ctrs)
            {
                if (constructorInfo.IsPublic)
                {
                    hasPublicConstructor = true;
                    break;
                }
            }
            Assert.IsFalse(hasPublicConstructor);
        }

        [TestMethod]
        public void MySingletonStaticInitializerTest()
        {
            MySingletonStaticInitialization s1 = MySingletonStaticInitialization.Instance;
            MySingletonStaticInitialization s2 = MySingletonStaticInitialization.Instance;

            Assert.AreSame(s1, s2);
        }

        [TestMethod]
        public void MySingletonStaticInitializationNoPublicConstructorsTest()
        {
            var singleton = typeof(MySingletonStaticInitialization);
            var ctrs = singleton.GetConstructors();
            var hasPublicConstructor = false;
            foreach (var constructorInfo in ctrs)
            {
                if (constructorInfo.IsPublic)
                {
                    hasPublicConstructor = true;
                    break;
                }
            }
            Assert.IsFalse(hasPublicConstructor);
        }

        [TestMethod]
        public void MySingletonMultithreadedTest()
        {
            MySingletonMultithreaded s1 = MySingletonMultithreaded.Instance;
            MySingletonMultithreaded s2 = MySingletonMultithreaded.Instance;

            Assert.AreSame(s1, s2);
        }

        [TestMethod]
        public void MySingletonMultithreadedNoPublicConstructorsTest()
        {
            var singleton = typeof(MySingletonMultithreaded);
            var ctrs = singleton.GetConstructors();
            var hasPublicConstructor = false;
            foreach (var constructorInfo in ctrs)
            {
                if (constructorInfo.IsPublic)
                {
                    hasPublicConstructor = true;
                    break;
                }
            }
            Assert.IsFalse(hasPublicConstructor);
        }

        [TestMethod]
        public void MySingletonLazyTest()
        {
            MySingletonLazy s1 = MySingletonLazy.Instance;
            MySingletonLazy s2 = MySingletonLazy.Instance;

            Assert.AreSame(s1, s2);
        }

        [TestMethod]
        public void MySingletonLazyNoPublicConstructorsTest()
        {
            var singleton = typeof(MySingletonLazy);
            var ctrs = singleton.GetConstructors();
            var hasPublicConstructor = false;
            foreach (var constructorInfo in ctrs)
            {
                if (constructorInfo.IsPublic)
                {
                    hasPublicConstructor = true;
                    break;
                }
            }
            Assert.IsFalse(hasPublicConstructor);
        }
    }
}
