using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
    [TestClass]
    public class BasicUITest : Session
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestInitialize]
        public void Clear()
        {

        }

        [TestMethod]
        public void NavigateMainTabs()
        {
            Console.WriteLine("Trying to Find Home Button");
            session.FindElementByName("ButtonShowRuntimeVersionInfo").Click();
        }
    }
}
