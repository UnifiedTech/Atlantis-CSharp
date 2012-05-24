namespace Atlantis.TestApp
{
    using Atlantis;

    using System;

    [Application(ApplicationUsage.Window, CreateUserAppData = true, CreateCommonAppData = true)]
    public class TestEntry
    {
        #region Methods

        public static void Main(string[] args)
        {
            Framework.Run<TestEntry>();
        }

        #endregion
    }
}