using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using System.Linq;

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
            string locatorSticker = ".//div[contains(@class,'sticker')]";
            
            IList<IWebElement> products = driver.FindElements(By.XPath(locator));

            if(AreElementsPresent(By.XPath(locator)))
            {
                for (int i = 0; i < products.Count; i++)
                {
                    Assert.IsTrue(AreElementsPresent(products, locatorSticker, i));
                    products = driver.FindElements(By.XPath(locator));
                }
            }
        }

       

    }
}
