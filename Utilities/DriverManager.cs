using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SpecFlowDemo.Utilities
{
    public class DriverManager
    {
        private static IWebDriver? _driver;

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                options.AddArgument("--disable-blink-features=AutomationControlled");
                options.AddExcludedArgument("enable-automation");
                options.AddArgument("--disable-notifications");
                options.AddUserProfilePreference(
                    "credentials_enable_service", false);

                // CI environment — headless mode
                if (Environment.GetEnvironmentVariable("CI") == "true")
                {
                    options.AddArgument("--headless=new");
                    options.AddArgument("--no-sandbox");
                    options.AddArgument("--disable-dev-shm-usage");
                    options.AddArgument("--disable-gpu");
                    options.AddArgument("--window-size=1920,1080");
                    Console.WriteLine(
                        "[DRIVER] Headless mode — CI environment");
                }
                else
                {
                    Console.WriteLine(
                        "[DRIVER] Normal mode — Local environment");
                }

                string driverPath = Directory.GetCurrentDirectory();
                var service = ChromeDriverService
                    .CreateDefaultService(driverPath);
                service.HideCommandPromptWindow = true;

                _driver = new ChromeDriver(service, options);
                _driver.Manage().Window.Maximize();
                _driver.Manage().Timeouts().ImplicitWait =
                    TimeSpan.FromSeconds(10);
                _driver.Manage().Timeouts().PageLoad =
                    TimeSpan.FromSeconds(60);
            }
            return _driver;
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        public static bool IsDriverActive()
        {
            return _driver != null;
        }
    }
}