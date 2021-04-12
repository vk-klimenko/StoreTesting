using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

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
        /// <summary>
        /// Проверить, что открывается правильная страница товара
        /// </summary>
        [Test]
        public void CheckCorrectProductPage()
        {
            driver.Url = baseUrl;

            driver.Url = baseUrl;

            Dictionary<string, string> dictMainProduct = new Dictionary<string, string>();
            Dictionary<string, string> dictPageCartProduct = new Dictionary<string, string>(); ;

            // Получаем данные товара с главной страницы
            IList<IWebElement> itemsProd = driver.FindElements(By.CssSelector("#box-campaigns a.link"));
            foreach (IWebElement product in itemsProd)
            {
                // Имя товара
                dictMainProduct.Add("name", product.FindElement(By.CssSelector("div.name")).GetAttribute("textContent").Trim());
                // Обычна цена
                dictMainProduct.Add("priceRegular", product.FindElement(By.CssSelector("div.price-wrapper s")).GetAttribute("textContent").Trim());
                // Аукционная цена
                dictMainProduct.Add("priceCampaign", product.FindElement(By.CssSelector("div.price-wrapper strong")).GetAttribute("textContent").Trim());
                // Цвет обычной цены rgba(119, 119, 119, 1)
                dictMainProduct.Add("priceColorRegular", product.FindElement(By.CssSelector("div.price-wrapper s")).GetCssValue("color").Replace("rgba(","").TrimEnd(')'));
                // Перечеркнута обычная цена text-decoration-line: line-through
                dictMainProduct.Add("priceLineThrough", product.FindElement(By.CssSelector("div.price-wrapper s")).GetCssValue("text-decoration-line").Trim());
                // Аукционная цена font-bold
                dictMainProduct.Add("priceBold", product.FindElement(By.CssSelector("div.price-wrapper strong")).GetCssValue("font-bold").Trim());
                // Аукционная цена цвет rgba(204, 0, 0, 1)
                dictMainProduct.Add("priceColorCampaign", product.FindElement(By.CssSelector("div.price-wrapper strong")).GetCssValue("color").Replace("rgba(", "").TrimEnd(')'));

                // Это тест. для задание под буквой д.
                // font size аукционная цена
                double aukceon_price = Convert.ToDouble(product.FindElement(By.CssSelector("div.price-wrapper")).GetCssValue("font-size").Replace("px", ""));
                // font size обычная цена
                double obichnaya_price = Convert.ToDouble(product.FindElement(By.CssSelector("div.price-wrapper s")).GetCssValue("font-size").Replace("em", ""));
                obichnaya_price *= aukceon_price;
                Assert.IsTrue(aukceon_price > obichnaya_price);

            }   


            /////////////////////////////////////////////////////////////////////////////
            // переход на карточку товара
            driver.Navigate().GoToUrl(driver.FindElement(By.CssSelector("#box-campaigns a.link")).GetAttribute("href"));

            // 

            IList<IWebElement> itemsCart = driver.FindElements(By.CssSelector("#box-product"));
            foreach (IWebElement cart in itemsCart)
            {
                dictPageCartProduct.Add("name", cart.FindElement(By.CssSelector("h1.title")).GetAttribute("textContent").Trim());
                dictPageCartProduct.Add("priceRegular", cart.FindElement(By.CssSelector("s.regular-price")).GetAttribute("textContent").Trim());
                dictPageCartProduct.Add("priceCampaign", cart.FindElement(By.CssSelector("strong.campaign-price")).GetAttribute("textContent").Trim());
                
                dictPageCartProduct.Add("priceColorRegular", cart.FindElement(By.CssSelector("s.regular-price")).GetCssValue("color").Trim());
            }
            

            // а) проверка на главной странице и на странице товара совпадает текст названия товара
            Assert.IsTrue((dictMainProduct["name"] == dictPageCartProduct["name"]));
            
            // б) на главной странице и на странице товара совпадают цены (обычная и акционная)
            Assert.IsTrue((dictMainProduct["priceRegular"] == dictPageCartProduct["priceRegular"]) && (dictMainProduct["priceCampaign"] == dictPageCartProduct["priceCampaign"]));

            // в) обычная цена зачёркнутая и серая (можно считать, что "серый" цвет это такой, у которого в RGBa представлении одинаковые значения для каналов R, G и B)
            Assert.IsTrue((dictMainProduct["priceLineThrough"] == "line-through") && dictMainProduct["priceColorRegular"] == "ВЕРНУТЬСЯ СЮДА!!!!");

            // г) акционная жирная и красная (можно считать, что "красный" цвет это такой, у которого в RGBa представлении каналы G и B имеют нулевые значения)
            Assert.IsTrue((dictMainProduct["priceBold"] == "") && (dictMainProduct["priceColorCampaign"] == "ВЕРНУТЬСЯ СЮДА!!!!"));

            // д) акционная цена крупнее, чем обычная (это тоже надо проверить на каждой странице независимо)
            // аукционная_цена_font_size = 18px
            // обычная цена_font_size = .price-wrapper * font-size(0.8em)
            
           

        }



    }
}
