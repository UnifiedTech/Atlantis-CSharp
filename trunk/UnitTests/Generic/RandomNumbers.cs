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
            const int MAX_COUNT = 25;

            List<int> randomInts = new List<int>();
            int seed = Convert.ToInt32(DateTime.MinValue.ToTimestamp());
            for (int i = 0; i < MAX_COUNT; ++i)
            {
                int x = new Random(seed).Next(0, Convert.ToInt32(DateTime.MinValue.ToTimestamp()));
                Assert.IsFalse(randomInts.Contains(x));
                randomInts.Add(x);

                seed = new Random().Next(Convert.ToInt32(DateTime.MinValue.ToTimestamp()), Convert.ToInt32(DateTime.MaxValue.ToTimestamp()));
            }
        }
    }
}
