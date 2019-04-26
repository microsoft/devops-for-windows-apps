using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace tests
{
    public class Session
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string AppId = "WpfCoreApp.DevOpsDemo.CD_px07m07ahrfnt!App";
        protected static WindowsDriver<WindowsElement> session;
        protected static WindowsDriver<WindowsElement> DesktopSession;
        private TestContext testContextInstance;
        private const int hostedAgentTimer = 500;

        public static void Setup(TestContext context)
        {
            if (session == null)
            {
                // Create a new session to bring up an instance of the Calculator application
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("deviceName", "WindowsPC");
                DesktopSession = null;
                appCapabilities.SetCapability("app", AppId);
                try
                {
                    Console.WriteLine("Trying to Launch App");
                    DesktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                }
                catch
                {
                    Console.WriteLine("Failed to attach to app session (expected).");
                }
                //Setting thread sleep timer. Hosted Agents take approximately 35 seconds to launch app
                Thread.Sleep(hostedAgentTimer);
                appCapabilities.SetCapability("app", "Root");
                DesktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Console.WriteLine("Attaching to WPFCoreApp Main Window");
                try
                {
                    //Trying to launch app (500 error expected in WAD v1.1)
                    var mainWindow1 = DesktopSession.FindElementByAccessibilityId("WPFCoreAppMainWindow");
                }
                catch
                {
                    Console.WriteLine("Switching to Desktop session.");

                }
                var mainWindow = DesktopSession.FindElementByAccessibilityId("WPFCoreAppMainWindow");
                Console.WriteLine("Getting Window Handle");
                var mainWindowHandle = mainWindow.GetAttribute("NativeWindowHandle");
                mainWindowHandle = (int.Parse(mainWindowHandle)).ToString("x"); // Convert to Hex
                appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", mainWindowHandle);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session);
                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                //   session.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3.5));
            }
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;

            }
        }

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
    }
}
