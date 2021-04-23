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
    public class ShopCartFrontendTests : BasesTest
    {
        /// <summary>
        /// Сценарий работы корзины.
        /// </summary>
        [Test]
        public void WorkingShopCartTest()
        {
            #region Задание
            /*  Сделайте сценарий для добавления товаров в корзину и удаления товаров из корзины.

            1. Открыть главную страницу, открыть первый товар из списка.
            2. Добавить в корзину товар.
            3. Подождать, пока счётчик товаров в корзине обновится.
            4. Вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара.
            5. Открыть корзину. Кликнуть по ссылке Checkout.
            6. Удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица
            */
            #endregion

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
                //1. найти старый элемент
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#box-checkout-summary td.item")));
                // 2.
                driver.FindElement(By.CssSelector("button[name='remove_cart_item']")).Click();
                // 3.
                wait.Until(ExpectedConditions.StalenessOf(product));
                // 4
                products = driver.FindElements(By.CssSelector("#box-checkout-summary td.item"));
            }

            //IWebElement btnRemoveItem;
            //while(driver.FindElements(By.CssSelector("button[name='remove_cart_item']")).Count() != 0)
            //{
            //    btnRemoveItem = driver.FindElement(By.CssSelector("button[name='remove_cart_item']"));
            //    driver.FindElement(By.CssSelector("button[name='remove_cart_item']")).Click();
            //    wait.Until(ExpectedConditions.StalenessOf(btnRemoveItem));
            //}
        }
    }
}
