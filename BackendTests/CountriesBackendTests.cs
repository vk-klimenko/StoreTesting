using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
namespace StoreTesting.BackendTests
{
    public class CountriesBackendTests: BasesTest
    {

        /// <summary>
        /// Проверка сортировки стран
        /// </summary>
        [Test]
        public void CheckingSortCountriesTest()
        {
            AdminPanelAuth("admin", "admin");

            driver.Navigate().GoToUrl($"{baseUrl}admin/?app=countries&doc=countries");

            IList<IWebElement> rows = driver.FindElements(By.CssSelector("tr.row"));

            Assert.IsTrue(AreElementsPresent(By.CssSelector("tr.row")));
            Assert.IsTrue(CheckingCountry(rows));
        }


        /// <summary>
        /// Проверка сортировки геозон
        /// </summary>
        [Test]
        public void CheckingSortGeoTest()
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
                foreach (IWebElement row in rowsGeoZone)
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
