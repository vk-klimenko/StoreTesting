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
               GoToPageURL(baseUrl);

                // Ожидание. Блок "Популярные товары"
                wait.Until(driver => GetElement(By.CssSelector("#box-most-popular")));

                // Переход на карточку товара
                GoToPageURL(By.CssSelector("#box-most-popular ul li a.link"));

                // Ожидание. Появление текста на кнопке.
                IWebElement textButton = GetElement(By.CssSelector("#box-product [name='add_cart_product']"));
                wait.Until(ExpectedConditions.TextToBePresentInElement(textButton, textButton.GetAttribute("textContent")));

                // Получаем количество товара в корзине 
                item = Convert.ToInt32(GetAttributeElement(By.CssSelector("#cart a.content span.quantity"),"textContent"));

                // Если нужно выбрать размер товара
                if (IsElementBoolen(By.Name("options[Size]")))
                {
                    SelectElement select = new SelectElement(GetElement(By.Name("options[Size]")));
                    select.SelectByIndex(rnd.Next(1, 4));
                }
                // Добавляем товар в корзину.
                PressClick(By.CssSelector("#box-product [name='add_cart_product']"));

                // Ожидание изменения кол-ва товара в корзине.
                item += 1;
                wait.Until(driver => GetAttributeElement(By.CssSelector("#cart a.content span.quantity"), "textContent") == Convert.ToString(item));

            }

            // Переход на главную страницу
            GoToPageURL(baseUrl);

            // Ожидание. Элемент "корзина"
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cart")));
            PressClick(By.CssSelector("#cart a.link"));

            // Ожидание. Список товаров.
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#box-checkout-summary td.item")));

            // Нажать на "карусель", чтобы не крутились товары
            if (IsElementBoolen(By.CssSelector("#box-checkout-cart ul.shortcuts li")))
                PressClick(By.CssSelector("#box-checkout-cart ul.shortcuts li"));

            IList<IWebElement> products = GetListElements(By.CssSelector("#box-checkout-summary td.item"));

            foreach (IWebElement product in products)
            {
                //1. найти старый элемент
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#box-checkout-summary td.item")));
                // 2.
                PressClick(By.CssSelector("button[name='remove_cart_item']"));
                // 3.
                wait.Until(ExpectedConditions.StalenessOf(product));
                // 4
                products = GetListElements(By.CssSelector("#box-checkout-summary td.item"));
            }

            //IWebElement btnRemoveItem;
            //while(GetListElements(By.CssSelector("button[name='remove_cart_item']")).Count() != 0)
            //{
            //    btnRemoveItem = driver.FindElement(By.CssSelector("button[name='remove_cart_item']"));
            //    driver.FindElement(By.CssSelector("button[name='remove_cart_item']")).Click();
            //    wait.Until(ExpectedConditions.StalenessOf(btnRemoveItem));
            //}
        }
    }
}
