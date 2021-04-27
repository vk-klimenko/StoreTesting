﻿using System;
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
    public class LogsBackendTests : BasesTest
    {
        [Test]
        public void CheckMessageBrowserLogTest()
        {
            #region Задание
            /*
             *  Проверьте отсутствие сообщений в логе браузера
                Сделайте сценарий, который проверяет, не появляются ли в логе браузера сообщения при открытии страниц в учебном приложении, 
                а именно -- страниц товаров в каталоге в административной панели.

                Сценарий должен состоять из следующих частей:

                1) зайти в админку
                2) открыть каталог, категорию, которая содержит товары (страница http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1)
                3) последовательно открывать страницы товаров и проверять, не появляются ли в логе браузера сообщения (любого уровня)
             */
            #endregion

            //td[@id='content']//tr[@class='row' and position() > 4]
            //td[@id='content']//tr[@class='row' and position() > 4]//a[not(@title='Edit')]


            AdminPanelAuth("admin", "admin");

            GoToPageURL("http://litecart/admin/?app=catalog&doc=catalog&category_id=1");

            IList<IWebElement> products = GetListElements(By.XPath(".//td[@id='content']//tr[@class='row' and position() > 4]//a[not(@title='Edit')]"));

            Assert.IsTrue(AreElementsPresent(By.XPath(".//td[@id='content']//tr[@class='row' and position() > 4]//a[not(@title='Edit')]")));

            foreach (IWebElement item in products)
            {
                string url = item.GetAttribute("href").Trim();
                GoToPageURL(url);
            }


        }
    }
}
