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
        public void ClickButton()
        {
            Console.WriteLine("Trying to Find Home Button");
            var button = session.FindElementByAccessibilityId("ButtonShowRuntimeVersionInfo");
            Assert.AreEqual("ControlType.Button", button.TagName);
            Assert.AreEqual("Show Runtime Version", button.Text);
            button.Click();

        }
    }
}
