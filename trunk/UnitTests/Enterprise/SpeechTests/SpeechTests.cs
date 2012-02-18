namespace UnitTests.Enterprise.SpeechTests
{
    using Atlantis.Enterprise.Voice;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class SpeechTests
    {
        [TestMethod]
        public void GeneratePasswordWithMultipleNumbers()
        {
            string passwd = Security.GeneratePassword("Gunnett");

            // TODO: Generate a password, then regex match it against a known match
            //      Then Assert.IsTrue(<match.Success>)
        }

        [TestMethod]
        public void GeneratePasswordWithSingleNumber()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void GeneratePasswordWorking()
        {
            string passwd = Security.GeneratePassword("Gunnett");
            Assert.IsNotNull(passwd, passwd);

            string passwd2 = Security.GeneratePassword("Gunnett");
            Assert.IsNotNull(passwd2, passwd2);
        }
    }
}
