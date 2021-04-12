using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

namespace StoreTesting
{
    [TestFixture]
    public class AdminPanelTest : BasesTest
    {
        /// <summary>
        /// Авторизация в админ панель
        /// </summary>
        [Test]
        public void LoginAdminPanel()
        {
            AdminPanelAuth("admin", "admin");
        }

        /// <summary>
        /// Авторизация в админ панель и чекбокс запомнить пользователя
        /// </summary>
        [Test]
        public void LoginRememberAdminPanel()
        {
            AdminPanelAuth("admin", "admin", true);
        }

        /// <summary>
        /// Выход из админ панели
        /// </summary>
        [Test]
        public void LogOutAdminPanel()
        {
            AdminPanelLogout("admin", "admin");
        }

        /// <summary>
        /// Клик по левому сайдбару и проверка H1
        /// </summary>
        [Test]
        public void LoginAdminPanelMenuSelect()
        {
            AdminPanelAuth("admin", "admin"); 
            IList<IWebElement> rows = driver.FindElement(By.CssSelector("ul#box-apps-menu")).FindElements(By.CssSelector("li#app-"));

            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Click();
                // проверка элемента h1
                Assert.IsTrue(IsElementPresent(By.CssSelector("td#content h1")));

                // проверка подразделов 
                if (AreElementsPresent(By.CssSelector("ul.docs")))
                {
                    IList<IWebElement> nestedItems = driver.FindElement(By.CssSelector("ul.docs")).FindElements(By.TagName("li"));
                    for (int x = 0; x < nestedItems.Count; x++)
                    {
                        nestedItems[x].Click();
                        // проверка элемента h1
                        Assert.IsTrue(IsElementPresent(By.CssSelector("td#content h1")));
                        nestedItems = driver.FindElement(By.CssSelector("ul.docs")).FindElements(By.TagName("li"));
                        
                    }
                }
                // обновляем элементы
                rows = driver.FindElements(By.CssSelector("li#app-"));
                
            }
        }


        /// <summary>
        /// Проверка сортировки стран
        /// </summary>
        [Test]
        public void CheckingSortCountries()
        {
            AdminPanelAuth("admin", "admin");

            driver.Navigate().GoToUrl($"{baseUrl}admin/?app=countries&doc=countries");

            // "tr.row td:nth-child(5n) a"

            string locator = "tr.row";

            IList<IWebElement> rows = driver.FindElements(By.CssSelector(locator));
            
            // 
            Assert.IsTrue(AreElementsPresent(By.CssSelector(locator)));

            // Проверка
            Assert.IsTrue(CheckingCountry(rows));

        }

        /// <summary>
        /// Проверка сортировки геозон
        /// </summary>
        [Test]
        public void CheckingSortGeo()
        {
            AdminPanelAuth("admin", "admin");

            driver.Navigate().GoToUrl($"{baseUrl}admin/?app=geo_zones&doc=geo_zones");

            IList<IWebElement> geoZone = driver.FindElements(By.CssSelector("[name = 'geo_zones_form'] tr.row"));

            for (int i = 0; i < geoZone.Count; i++)
            {

                driver.Url = geoZone[i].FindElement(By.CssSelector("tr.row td:nth-child(3n) a")).GetAttribute("href").Trim();

                IList<IWebElement> rowsGeoZone = driver.FindElements(By.XPath(".//table[@id='table-zones']//tr[position() > 1 and position() < last()]//select[contains(@name,'zone_code')]/option"));
                Assert.IsTrue(AreElementsPresent(By.XPath(".//table[@id='table-zones']//tr[position() > 1 and position() < last()]//select[contains(@name,'zone_code')]/option")));

                List<string> geoList = new List<string>();
                foreach(IWebElement row in rowsGeoZone)
                {
                    if (row.GetAttribute("selected") == "true")
                        geoList.Add(row.GetAttribute("innerText").Trim());
                }

                List<string> geoListSort = geoList;
                geoListSort.Sort();
                Assert.IsTrue(geoList.SequenceEqual(geoListSort));


                driver.Navigate().Back();
                
                geoList.Clear();
                geoListSort.Clear();

                geoZone = driver.FindElements(By.CssSelector("[name = 'geo_zones_form'] tr.row"));
            }

        }

    }
}
