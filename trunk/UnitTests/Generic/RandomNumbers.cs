namespace UnitTests.Generic
{
    using Atlantis.Linq;
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RandomNumbers
    {
        [TestMethod]
        public void TwentyFiveRandomNumbers()
        {
            const Int32 MAX_COUNT = 25;

            List<Int32> randomInts = new List<Int32>();
            Int32 seed = Convert.ToInt32(DateTime.MinValue.ToTimestamp());
            for (Int32 i = 0; i < MAX_COUNT; ++i)
            {
                Int32 x = new Random(seed).Next(0, Convert.ToInt32(DateTime.MinValue.ToTimestamp()));
                Assert.IsFalse(randomInts.Contains(x));
                randomInts.Add(x);

                seed = new Random().Next(Convert.ToInt32(DateTime.MinValue.ToTimestamp()), Convert.ToInt32(DateTime.MaxValue.ToTimestamp()));
            }
        }
    }
}
