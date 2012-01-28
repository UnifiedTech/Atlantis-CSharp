namespace UnitTests.Xcf
{
    using Atlantis.Enterprise.Xcf;
    using Atlantis.Enterprise.Xcf.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class XcfBuilderTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            XcfBuilder builder = new XcfBuilder();

            for (Int32 i = 0; i < 25; ++i)
            {
                XcfSection section = new XcfSection(String.Format("Section{0}", i));

                for (Int32 x = 0; x < 1000; ++x)
                {
                    section.AddKey(String.Format("item{0}", x), x);
                }

                Assert.AreEqual(1000, section.Keys.Count);
                builder.WriteSection(section);
            }
        }
    }
}
