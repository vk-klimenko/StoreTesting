using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

namespace StoreTesting
{
    [TestFixture]
    public class StoreEcomerc
    {
        private IWebDriver driver;
        
        private string url = "http://litecart/";

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void LoginAdminPanel()
        {
            driver.Url = $"{url}admin/";

            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

    }       
}
