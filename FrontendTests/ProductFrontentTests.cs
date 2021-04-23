using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Globalization;

namespace StoreTesting.FrontendTests
{
    public class ProductFrontentTests : BasesTest
    {

        /// <summary>
        /// Наличие стикеров на товарах
        /// </summary>
        [Test]
        public void AvailabilityStickersOnGoodsTest()
        {
            GoToPageURL($"{baseUrl}rubber-ducks-c-1/");
            string locator = ".//div[@id='box-category']//ul[contains(@class,'products')]/li";
            string locatorSticker = ".//div[contains(@class,'sticker')]";

            IList<IWebElement> products = GetListElements(By.XPath(locator));

            if (AreElementsPresent(By.XPath(locator)))
            {
                for (int i = 0; i < products.Count; i++)
                {
                    Assert.IsTrue(AreElementsPresent(products, locatorSticker, i));
                    products = GetListElements(By.XPath(locator));
                }
            }
        }


        /// <summary> 
        /// Проверить, что открывается правильная страница товара 
        /// </summary> 
        [Test]
        public void CheckCorrectProductPageTest()
        {
            GoToPageURL(baseUrl);

            Dictionary<string, string> dictMainProduct = new Dictionary<string, string>();
            Dictionary<string, string> dictPageCartProduct = new Dictionary<string, string>(); ;

            // Получаем данные товара с главной страницы 

            // Имя товара
            dictMainProduct.Add("name", GetAttributeElement(By.CssSelector("#box-campaigns a.link div.name"), "textContent"));

            // Обычна цена
            dictMainProduct.Add("priceRegular", GetAttributeElement(By.CssSelector("#box-campaigns a.link div.price-wrapper s"), "textContent"));

            // Аукционная цена 
            dictMainProduct.Add("priceCampaign", GetAttributeElement(By.CssSelector("#box-campaigns a.link div.price-wrapper strong"), "textContent"));

            // Цвет обычной цены rgba(119, 119, 119, 1) 
            // 
            dictMainProduct.Add("priceColorRegular", (GetCssValueElement(By.CssSelector("#box-campaigns a.link div.price-wrapper s"), "color")).Replace("rgba(", "").TrimEnd(')')); 

            // Перечеркнута обычная цена text-decoration-line: line-through 
            dictMainProduct.Add("priceLineThrough", GetCssValueElement(By.CssSelector("#box-campaigns a.link div.price-wrapper s"),"text-decoration-line"));

            // Аукционная цена font-bold 
            dictMainProduct.Add("priceBold", GetCssValueElement(By.CssSelector("#box-campaigns a.link div.price-wrapper strong"), "font-weight"));

            // Аукционная цена цвет rgba(204, 0, 0, 1) 
            dictMainProduct.Add("priceColorCampaign", (GetCssValueElement(By.CssSelector("#box-campaigns a.link div.price-wrapper strong"), "color")).Replace("rgba(", "").TrimEnd(')'));

            // Аукционная цена - размер шрифта
            dictMainProduct.Add("priceFontSizeCampaign", (GetCssValueElement(By.CssSelector("#box-campaigns div.price-wrapper strong.campaign-price"), "font-size")).Replace("px", "").Replace(".", ",").Trim());

            // Обычная цена - размер шрифта
            dictMainProduct.Add("priceFontSizeRegular", GetCssValueElement(By.CssSelector("#box-campaigns div.price-wrapper s.regular-price"), "font-size").Replace("px", "").Replace(".", ",").Trim());


            ///////////// ************************************************************************************  ////////////////////// 

            // переход на карточку товара 
            GoToPageURL(By.CssSelector("#box-campaigns a.link"));

            // Имя товара 
            dictPageCartProduct.Add("name", GetAttributeElement(By.CssSelector("#box-product h1.title"), "textContent"));

            // Обычна цена
            dictPageCartProduct.Add("priceRegular", GetAttributeElement(By.CssSelector("#box-product s.regular-price"), "textContent"));

            // Аукционная цена
            dictPageCartProduct.Add("priceCampaign", GetAttributeElement(By.CssSelector("#box-product strong.campaign-price"), "textContent"));

            // Цвет обычной цены
            dictPageCartProduct.Add("priceColorRegular", (GetCssValueElement(By.CssSelector("#box-product s.regular-price"), "color")).Replace("rgba(", "").TrimEnd(')'));

            // Аукционная цена - размер шрифта
            dictPageCartProduct.Add("priceFontSizeCampaign", (GetCssValueElement(By.CssSelector("#box-product div.price-wrapper strong.campaign-price"), "font-size")).Replace("px", "").Replace(".", ",").Trim());

            // Обычная цена - размер шрифта
            dictPageCartProduct.Add("priceFontSizeRegular", (GetCssValueElement(By.CssSelector("#box-product div.price-wrapper s.regular-price"), "font-size")).Replace("px", "").Replace(".", ",").Trim());

            ////////////////// ПРОВЕРКИ /////////////////////////////

            // а) проверка на главной странице и на странице товара совпадает текст названия товара 
            Assert.IsTrue((dictMainProduct["name"] == dictPageCartProduct["name"]));

            // б) на главной странице и на странице товара совпадают цены (обычная и акционная) 
            Assert.IsTrue((dictMainProduct["priceRegular"] == dictPageCartProduct["priceRegular"]) && (dictMainProduct["priceCampaign"] == dictPageCartProduct["priceCampaign"]));

            // в) обычная цена зачёркнутая и серая (можно считать, что "серый" цвет это такой, у которого в RGBa представлении одинаковые значения для каналов R, G и B) 
            Assert.IsTrue((dictMainProduct["priceLineThrough"] == "line-through") && (dictMainProduct["priceColorRegular"].Split(',')[0].Trim() == dictMainProduct["priceColorRegular"].Split(',')[1].Trim()) && (dictMainProduct["priceColorRegular"].Split(',')[1].Trim() == dictMainProduct["priceColorRegular"].Split(',')[2].Trim()));

            // г) акционная жирная и красная (можно считать, что "красный" цвет это такой, у которого в RGBa представлении каналы G и B имеют нулевые значения) 
            Assert.IsTrue((dictMainProduct["priceBold"] == "bold" || (dictMainProduct["priceBold"] == "700") && (dictMainProduct["priceColorCampaign"].Split(',')[1].Trim() == dictMainProduct["priceColorCampaign"].Split(',')[2].Trim())));

            // д) акционная цена крупнее, чем обычная (это тоже надо проверить на каждой странице независимо) 
            // На главной странице
            Assert.IsTrue(Convert.ToDouble(dictMainProduct["priceFontSizeCampaign"]) > Convert.ToDouble(dictMainProduct["priceFontSizeRegular"]));
            // На странице карточки товара
            Assert.IsTrue(Convert.ToDouble(dictPageCartProduct["priceFontSizeCampaign"]) > Convert.ToDouble(dictPageCartProduct["priceFontSizeRegular"]));
        }
    }
}
