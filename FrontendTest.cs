using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

namespace StoreTesting
{
    [TestFixture]
    public class FrontendTest:BasesTest
    {
        /// <summary>
        /// Наличие стикеров на товарах
        /// </summary>
        [Test]
        public void AvailabilityStickersOnGoods()
        {
            driver.Url = $"{baseUrl}rubber-ducks-c-1/";
            string locator = ".//div[@id='box-category']//ul[contains(@class,'products')]/li";

            IList<IWebElement> products = driver.FindElements(By.XPath(locator));

            if(AreElementsPresent(By.XPath(locator)))
            {
                for (int i = 0; i < products.Count; i++)
                {
                    
                    Assert.IsTrue(IsElementPresent(By.XPath(".//div[contains(@class,'sticker')]"), products, i));

                    products = driver.FindElements(By.XPath(locator));
                }
            }
        }
        
    }
}
