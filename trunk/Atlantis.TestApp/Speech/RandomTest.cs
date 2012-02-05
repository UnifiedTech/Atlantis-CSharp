using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.TestApp.Speech
{
    class RandomTest
    {
        public static void Main()
        {
            const Int32 max = 25;
            Int32 last = 0;
            for (Int32 i = 0; i < max; ++i)
            {
                Int32 seed = new Random().Next(Convert.ToInt32(DateTime.MinValue), Convert.ToInt32(DateTime.MaxValue));

            }

            Console.ReadKey(true);
        }
    }
}
