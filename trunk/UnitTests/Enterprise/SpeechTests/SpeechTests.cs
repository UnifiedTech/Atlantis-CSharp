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
            String passwd = Security.GenerateVoicePassword("Gunnett", 4);

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
            String passwd = Security.GenerateVoicePassword("Gunnett", 5);
            Assert.IsNotNull(passwd, passwd);

            String passwd2 = Security.GenerateVoicePassword("Gunnett");
            Assert.IsNotNull(passwd2, passwd2);
        }
    }
}
