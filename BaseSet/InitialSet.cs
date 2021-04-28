using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace StoreTesting
{
    public class InitialSet
    {
        protected IWebDriver driver;
        protected IWebDriver driverLog;
        protected WebDriverWait wait;
        protected string baseUrl = "http://litecart/";
        protected Random rnd;


        [SetUp]
        public void Start()
        {
            ChromeOptions options = new ChromeOptions();
            options.SetLoggingPreference("browser", LogLevel.All);
            driver = new ChromeDriver(options);

            //driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Stop()
        {
            //driverLog.Quit();
            //driverLog = null;

            driver.Quit();
            driver = null;
        }
    }
}
