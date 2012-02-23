namespace UnitTests.IniTests
{
    using Atlantis.IO;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IniTests
    {
        // Test File from pastebin.com -> Search -> INI File


        [TestMethod]
        public void IniFileNotNullFromInitFileLoad()
        {
            IniFIle ini = IniFIle.Load("test.ini");
            Assert.IsNotNull(ini);
        }

        [TestMethod]
        public void IniFileNotNullFromNew()
        {
            IniFIle ini = new IniFIle("test.ini");
            Assert.IsNotNull(ini);
        }

        [TestMethod]
        public void IniFileHasSixSections()
        {
            IniFIle ini = IniFIle.Load("test.ini");
            Assert.AreEqual(6, ini.Count);
        }

        [TestMethod]
        public void KeyExistsTest()
        {
            IniFIle ini = IniFIle.Load("test.ini");

            string item = ini["emc", "machine"];
            Assert.IsFalse(String.IsNullOrEmpty(item));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void KeyDoesntExist()
        {
            IniFIle ini = IniFIle.Load("test.ini");

            var item = ini["general"];
            Assert.IsNotNull(item);
        }
    }
}
