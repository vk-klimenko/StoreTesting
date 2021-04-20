﻿using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Globalization;

namespace StoreTesting
{
    [TestFixture]
    public class FrontendTest:BasesTest
    {
        /// <summary>
        /// Наличие стикеров на товарах
        /// </summary>
        [Test]
        public void AvailabilityStickersOnGoodsTest()
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
        public void CheckCorrectProductPageTest()
        {
            driver.Url = baseUrl;
            
            Dictionary<string, string> dictMainProduct = new Dictionary<string, string>();
            Dictionary<string, string> dictPageCartProduct = new Dictionary<string, string>(); ;

            // Получаем данные товара с главной страницы 

            // Имя товара
            dictMainProduct.Add("name", driver.FindElement(By.CssSelector("#box-campaigns a.link div.name")).GetAttribute("textContent").Trim());
              
            // Обычна цена
            dictMainProduct.Add("priceRegular", driver.FindElement(By.CssSelector("#box-campaigns a.link div.price-wrapper s")).GetAttribute("textContent").Trim());
            
            // Аукционная цена 
            dictMainProduct.Add("priceCampaign", driver.FindElement(By.CssSelector("#box-campaigns a.link div.price-wrapper strong")).GetAttribute("textContent").Trim());
            
            // Цвет обычной цены rgba(119, 119, 119, 1) 
            dictMainProduct.Add("priceColorRegular", driver.FindElement(By.CssSelector("#box-campaigns a.link div.price-wrapper s")).GetCssValue("color").Replace("rgba(", "").TrimEnd(')'));

            // Перечеркнута обычная цена text-decoration-line: line-through 
            dictMainProduct.Add("priceLineThrough", driver.FindElement(By.CssSelector("#box-campaigns a.link div.price-wrapper s")).GetCssValue("text-decoration-line").Trim());

             // Аукционная цена font-bold 
            dictMainProduct.Add("priceBold", driver.FindElement(By.CssSelector("#box-campaigns a.link div.price-wrapper strong")).GetCssValue("font-weight").Trim());

             // Аукционная цена цвет rgba(204, 0, 0, 1) 
             dictMainProduct.Add("priceColorCampaign", driver.FindElement(By.CssSelector("#box-campaigns a.link div.price-wrapper strong")).GetCssValue("color").Replace("rgba(", "").TrimEnd(')'));

            // Аукционная цена - размер шрифта
            dictMainProduct.Add("priceFontSizeCampaign", driver.FindElement(By.CssSelector("#box-campaigns div.price-wrapper strong.campaign-price")).GetCssValue("font-size").Replace("px", "").Replace(".",",").Trim());

            // Обычная цена - размер шрифта
            dictMainProduct.Add("priceFontSizeRegular", driver.FindElement(By.CssSelector("#box-campaigns div.price-wrapper s.regular-price")).GetCssValue("font-size").Replace("px", "").Replace(".", ",").Trim());


            ///////////// ************************************************************************************  ////////////////////// 

            // переход на карточку товара 
            driver.Navigate().GoToUrl(driver.FindElement(By.CssSelector("#box-campaigns a.link")).GetAttribute("href"));
            
            // Имя товара 
            dictPageCartProduct.Add("name", driver.FindElement(By.CssSelector("#box-product h1.title")).GetAttribute("textContent").Trim());
            
            // Обычна цена
            dictPageCartProduct.Add("priceRegular", driver.FindElement(By.CssSelector("#box-product s.regular-price")).GetAttribute("textContent").Trim());
            
            // Аукционная цена
            dictPageCartProduct.Add("priceCampaign", driver.FindElement(By.CssSelector("#box-product strong.campaign-price")).GetAttribute("textContent").Trim());
            
            // Цвет обычной цены
            dictPageCartProduct.Add("priceColorRegular", driver.FindElement(By.CssSelector("#box-product s.regular-price")).GetCssValue("color").Replace("rgba(", "").TrimEnd(')'));
            
            // Аукционная цена - размер шрифта
            dictPageCartProduct.Add("priceFontSizeCampaign", driver.FindElement(By.CssSelector("#box-product div.price-wrapper strong.campaign-price")).GetCssValue("font-size").Replace("px","").Replace(".", ",").Trim());

            // Обычная цена - размер шрифта
            dictPageCartProduct.Add("priceFontSizeRegular", driver.FindElement(By.CssSelector("#box-product div.price-wrapper s.regular-price")).GetCssValue("font-size").Replace("px", "").Replace(".", ",").Trim());

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

        /// <summary>
        /// Сценарий регистрации новых пользователей
        /// </summary>
        [Test]
        public void UserRegistrationTest()
        {
            // Сценарий должен состоять из следующих частей:
            //1) регистрация новой учётной записи с достаточно уникальным адресом электронной почты(чтобы не конфликтовало с ранее созданными пользователями, в том числе при предыдущих запусках того же самого сценария),
            //2) выход(logout), потому что после успешной регистрации автоматически происходит вход,
            //3) повторный вход в только что созданную учётную запись,
            //4) и ещё раз выход.
            //В качестве страны выбирайте United States, штат произвольный.При этом формат индекса --пять цифр.
            //Можно оформить сценарий либо как тест, либо как отдельный исполняемый файл.
            //Проверки можно никакие не делать, только действия - заполнение полей, нажатия на кнопки и ссылки.Если сценарий дошёл до конца, то есть созданный пользователь смог выполнить вход и выход-- значит создание прошло успешно.

            driver.Navigate().GoToUrl($"{baseUrl}en/create_account");
            CreateUsers user = new CreateUsers("Masha", "Sasha", "Mankheton 77", "password2", "Las-Vegas");

            // 1. Запонение формы
            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[2]/td[1]/input[@name='firstname']"), user.FirstName);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[2]/td[1]/input[@name='firstname']")).GetAttribute("value"));
            
            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[2]/td[2]/input[@name='lastname']"), user.LastName);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[2]/td[2]/input[@name='lastname']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[3]/td[1]/input[@name='address1']"), user.Address);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[3]/td[1]/input[@name='address1']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[4]/td[1]/input[@name='postcode']"), user.Postcode);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[4]/td[1]/input[@name='postcode']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[4]/td[2]/input[@name='city']"), user.City);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[4]/td[2]/input[@name='city']")).GetAttribute("value"));

            // ---------------------------------------------------------------------------------------------------
            // Клик по элементу выпадающего списка
            driver.FindElement(By.CssSelector("span [role='presentation']")).Click();
            // В откр. списке ищем поле ввода
            driver.FindElement(By.CssSelector("input.select2-search__field")).SendKeys("United States" + Keys.Enter);
            // ---------------------------------------------------------------------------------------------------

            DropDownList(By.CssSelector("select[name='zone_code']"), new Random().Next(1, 65));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[6]/td[1]/input[@name='email']"), user.Email);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[6]/td[1]/input[@name='email']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[6]/td[2]/input[@name='phone']"), user.Phone);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[6]/td[2]/input[@name='phone']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[8]/td[1]/input[@name='password']"), user.Password);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[1]/input[@name='password']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[8]/td[2]/input[@name='confirmed_password']"), user.Password);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[2]/input[@name='confirmed_password']")).GetAttribute("value"));

            //driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[1]/input[@name='password']")).SendKeys(Keys.Control + "c" + user.Password);
            //driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[2]/input[@name='confirmed_password']")).SendKeys(Keys.Control + "v");

            PressButton(By.XPath(".//div[@id='create-account']//tr[9]//button[@name='create_account']"));
            
            // 2. Logout из аккаунта

            user.LogoutAccount(driver, By.XPath(".//div[@id='box-account']//li[last()]/a[.='Logout']"));
            Assert.IsTrue(driver.FindElement(By.XPath(".//div[@id='box-account-login']/h3[@class='title']")).GetAttribute("textContent") == "Login");

            // 3. Авторизация под пользователем

            user.EnterDataInput(driver, By.XPath(".//form[@name='login_form']//input[@name='email']"), user.Email);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//form[@name='login_form']//input[@name='email']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//form[@name='login_form']//input[@name='password']"), user.Password);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//form[@name='login_form']//input[@name='password']")).GetAttribute("value"));

            PressButton(By.XPath(".//form[@name='login_form']//button[@name='login']"));
            
            // Проверяем, что зашли под пользователем
            Assert.IsTrue(driver.FindElement(By.XPath(".//div[@id='box-account']/h3[@class='title']")).GetAttribute("textContent") == "Account");

            // 4. Logout из аккаунта

            user.LogoutAccount(driver, By.XPath(".//div[@id='box-account']//li[last()]/a[.='Logout']"));

            // Проверяем, что вышли под пользователем
            Assert.IsTrue(driver.FindElement(By.XPath(".//div[@id='box-account-login']/h3[@class='title']")).GetAttribute("textContent") == "Login");
        }

        /// <summary>
        /// Сценарий работы корзины.
        /// </summary>
        [Test]
        public void WorkingShopCartTest()
        {
            /*  Сделайте сценарий для добавления товаров в корзину и удаления товаров из корзины.

            1. Открыть главную страницу, открыть первый товар из списка.
            2. Добавить в корзину товар.
            3. Подождать, пока счётчик товаров в корзине обновится.
            4. Вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара.
            5. Открыть корзину. Кликнуть по ссылке Checkout.
            6. Удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица
            */

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int item = 0;

            for (int i = 0; i < 3; i++)
            {
                driver.Navigate().GoToUrl(baseUrl);
                
                // Ожидание. Блок "Популярные товары"
                wait.Until(driver => driver.FindElement(By.CssSelector("#box-most-popular")));

                // Переход на карточку товара
                driver.Navigate().GoToUrl(driver.FindElement(By.CssSelector("#box-most-popular ul li a.link")).GetAttribute("href").Trim());

                // Ожидание. Появление текста на кнопке.
                IWebElement textButton = driver.FindElement(By.CssSelector("#box-product [name='add_cart_product']"));
                wait.Until(ExpectedConditions.TextToBePresentInElement(textButton, textButton.GetAttribute("textContent")));

                // Получаем количество товара в корзине 
                item = Convert.ToInt32(driver.FindElement(By.CssSelector("#cart a.content span.quantity")).GetAttribute("textContent").Trim());

                // Если нужно выбрать размер товара
                if (IsElementBoolen(By.Name("options[Size]")))
                {
                    SelectElement select = new SelectElement(driver.FindElement(By.Name("options[Size]")));
                    select.SelectByIndex(new Random().Next(1, 4));
                }
                // Добавляем товар в корзину.
                driver.FindElement(By.CssSelector("#box-product [name='add_cart_product']")).Click();

                // Ожидание изменения кол-ва товара в корзине.
                item += 1;
                wait.Until(driver => driver.FindElement(By.CssSelector("#cart a.content span.quantity")).GetAttribute("textContent") == Convert.ToString(item));
            
            }

            // Переход на главную страницу
            driver.Navigate().GoToUrl(baseUrl);
           
            // Ожидание. Элемент "корзина"
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cart")));
            driver.FindElement(By.CssSelector("#cart a.link")).Click();
            
            // Ожидание. Список товаров.
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#box-checkout-summary td.item")));
            
            // Нажать на "карусель", чтобы не крутились товары
            if (IsElementBoolen(By.CssSelector("#box-checkout-cart ul.shortcuts li")))
                driver.FindElement(By.CssSelector("#box-checkout-cart ul.shortcuts li")).Click();

            IList<IWebElement> products = driver.FindElements(By.CssSelector("#box-checkout-summary td.item"));
            foreach (IWebElement product in products)
            {
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[name='remove_cart_item']")));
                driver.FindElement(By.CssSelector("button[name='remove_cart_item']")).Click();
                
                // Ожидание обновления таблицы
                wait.Until(ExpectedConditions.StalenessOf(product));
            }

            
        }

    }
}
