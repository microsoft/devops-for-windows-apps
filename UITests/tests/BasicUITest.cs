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
            Assert.AreEqual("Show Runtime Info", button.Text);
            button.Click();
            System.Threading.Thread.Sleep(200);
        }

        [TestMethod]
        public void ShowHideRuntimeInfo()
        {
            Console.WriteLine("Trying to Find Home Button");
            var button = session.FindElementByAccessibilityId("ButtonShowRuntimeVersionInfo");
            Assert.AreEqual("ControlType.Button", button.TagName);
            Assert.AreEqual("Hide Runtime Info", button.Text);
            button.Click();

            System.Threading.Thread.Sleep(200);
            Assert.AreEqual("Show Runtime Info", button.Text);
            button.Click();
            
            System.Threading.Thread.Sleep(200);
            Assert.AreEqual("Hide Runtime Info", button.Text);
            
            var labelRuntimeInfo = session.FindElementByAccessibilityId("RuntimeVersionInfo");
            Assert.IsTrue(labelRuntimeInfo.Text.Contains("4.8"), "FX 4.8 not found");
            
            button.Click();
            System.Threading.Thread.Sleep(200);
         
            Assert.AreEqual("Show Runtime Info", button.Text);
            button.Click();
            System.Threading.Thread.Sleep(200);
            Assert.AreEqual("Hide Runtime Info", button.Text);
        }

    }
}
