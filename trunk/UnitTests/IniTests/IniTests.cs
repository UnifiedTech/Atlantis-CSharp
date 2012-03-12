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
            IniFile ini = IniFile.Load("test.ini");
            Assert.IsNotNull(ini);
        }

        [TestMethod]
        public void IniFileNotNullFromNew()
        {
            IniFile ini = new IniFile("test.ini");
            Assert.IsNotNull(ini);
        }

        [TestMethod]
        public void IniFileHasSixSections()
        {
            IniFile ini = IniFile.Load("test.ini");
            Assert.AreEqual(6, ini.Count);
        }

        [TestMethod]
        public void KeyExistsTest()
        {
            IniFile ini = IniFile.Load("test.ini");

            string item = ini["emc", "machine"];
            Assert.IsFalse(String.IsNullOrEmpty(item));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void KeyDoesntExist()
        {
            IniFile ini = IniFile.Load("test.ini");

            var item = ini["general"];
            Assert.IsNotNull(item);
        }
    }
}
