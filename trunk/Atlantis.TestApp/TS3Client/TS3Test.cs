namespace Atlantis.TestApp.TS3Client
{
    using Atlantis.Net.GameQuery.Teamspeak;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TS3Test
    {
        public static void Main(string[] args)
        {
            Console.Title = "TS3Client Test";

            TS3Client client = new TS3Client("tspub.unifiedtech.org", 9987, 10011, false, "serveradmin", "password");
            Boolean ret = client.Connect();


            Console.ReadKey();
        }
    }
}
