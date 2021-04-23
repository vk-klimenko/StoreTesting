using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;

namespace StoreTesting
{
    public class BasesTest : InitialSet
    {
        /// <summary>
        /// Поиск элементов
        /// </summary>
        /// <param name="locator">Локатор поиска</param>
        /// <returns>Если кол-во больше нуля - TRUE</returns>
        protected bool AreElementsPresent(By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }
        /// <summary>
        /// Поиск элементов в цикле
        /// </summary>
        /// <param name="locator">Локатор</param>
        /// <param name="list">Список элементов</param>
        /// <param name="i">Индекс</param>
        /// <returns></returns>
        protected bool AreElementsPresent(IList<IWebElement> list, string locator, int i)
        {
            return (list[i].FindElements(By.XPath(locator))).Count == 1;
        }
        /// <summary>
        /// Поиск одного элемента
        /// </summary>
        /// <param name="locator">Локатор поиска</param>
        /// <returns>Возвращает исключение NoSuchElementException</returns>
        protected bool IsElementPresent(By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception($"Not found locator: {locator}", ex);
            }
        }
        /// <summary>
        /// Поиск элемента в цикле
        /// </summary>
        /// <param name="locator">Локатор</param>
        /// <param name="list">Список элементов</param>
        /// <param name="i">Индекс</param>
        /// <returns>True - элемент найден, Исключение NoSuchElementException - элемент не найден</returns>
        protected bool IsElementPresent(By locator, IList<IWebElement> list, int i)
        {
            try
            {
                list[i].FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception($"Not found locator: {locator}", ex);
            }
        }
        /// <summary>
        /// Поиск одного элемента 
        /// </summary>
        /// <param name="locator">Локатор поиска</param>
        /// <returns>Возвращает boolean </returns>
        protected bool IsElementBoolen(By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Авторизация в админ панель
        /// </summary>
        protected void AdminPanelAuth(string name, string password)
        {
            driver.Url = $"{baseUrl}admin/";
            driver.FindElement(By.Name("username")).SendKeys(name);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("login")).Click();
        }
        /// <summary>
        ///Авторизация в админ панель + запомнить 
        /// </summary>
        /// <param name="name">логин</param>
        /// <param name="password">пароль</param>
        /// <param name="check">Boolean - запомнить пользователя</param>
        protected void AdminPanelAuth(string name, string password, bool check=false)
        {
            driver.Url = $"{baseUrl}admin/";
            driver.FindElement(By.Name("username")).SendKeys(name);
            driver.FindElement(By.Name("password")).SendKeys(password);
            if (check)
                driver.FindElement(By.XPath(".//div[@class='content']//label/input")).Click();
            driver.FindElement(By.Name("login")).Click();
        }
        /// <summary>
        /// Выход из админ панели
        /// </summary>
        /// <param name="name">имя пользователя</param>
        /// <param name="password">пароль пользователя</param>
        protected void AdminPanelLogout(string name, string password)
        {
            string locator = ".//div[@class='header']/a[@title='Logout']";
            driver.Url = $"{baseUrl}admin/";
            if (IsElementBoolen(By.XPath(locator)))
            {
                driver.FindElement(By.XPath(locator)).Click();
            }
            else
            {
                AdminPanelAuth(name, password);
                driver.FindElement(By.XPath(locator)).Click();
            }
            
        }

        /// <summary>
        /// Проверяем, что страны расположены в алфавитном порядке.
        /// </summary>
        /// <returns>True - списки одинаковы, False - не одинаковы</returns>
        protected bool CheckingCountry(IList<IWebElement> rows)
        {
            List<string> countryList = new List<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                countryList.Add(rows[i].FindElement(By.CssSelector("td:nth-child(5n) a")).GetAttribute("innerText").Trim());

                // проверяем есть ли у страны доп. зоны
                int temp = Convert.ToInt32(rows[i].FindElement(By.CssSelector("td:nth-child(6n)")).GetAttribute("innerText").Trim());
                if (temp > 0)
                {
                    List<string> byZones = new List<string>();
                    
                    driver.Url = rows[i].FindElement(By.CssSelector("td:nth-child(5n) a")).GetAttribute("href");

                    // Получаем список зон и делаем срез с помощью xpath [position() > 1 and position() < last()]
                    IList<IWebElement> rowsZones = driver.FindElements(By.XPath("//table[@id='table-zones']//tr[position() > 1 and position() < last()]"));
                    
                    AreElementsPresent(By.XPath("//table[@id='table-zones']//tr[position()>1 and position() < last()]"));
                    
                    foreach (IWebElement rowZone in rowsZones)
                    {
                        byZones.Add(rowZone.FindElement(By.XPath(".//td[3]")).GetAttribute("innerText").Trim());
                    }
                    List<string> byZonesSort = byZones;

                    // Проверка доп зон на расположение в алфавитном порядке
                    Assert.IsTrue(byZones.SequenceEqual(byZonesSort));
                    driver.Navigate().Back();
                    // Обновляем элементы
                    rows = driver.FindElements(By.CssSelector("tr.row"));
                }
            }
            List<string> countryListSort = countryList;
            countryListSort.Sort();
            
            return countryList.SequenceEqual(countryListSort); 
        }
        /// <summary>
        /// Нажать на кнопку
        /// </summary>
        /// <param name="locator">Локатор</param>
        protected void PressClick(By locator)
        {
            IsElementPresent(locator);
            driver.FindElement(locator).Click();
        }
        /// <summary>
        /// Выбрать из выпадающего списка
        /// </summary>
        /// <param name="locator">Локатор</param>
        /// <param name="selectValue">Значение из выпадающего списка по "By value"</param>
        protected void DropDownList(By locator, string selectValue)
        {
            SelectElement selectElement = new SelectElement(driver.FindElement(locator));
            
            selectElement.SelectByValue(selectValue);
        }
        /// <summary>
        /// Выбрать из выпадающего списка
        /// </summary>
        /// <param name="locator">Локатор</param>
        /// <param name="selectByIndex">Значение из выпадающего списка по "By index"</param>
        protected void DropDownList(By locator, int selectByIndex)
        {
            SelectElement selectElement = new SelectElement(driver.FindElement(locator));
            selectElement.SelectByIndex(selectByIndex);
        }
        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <param name="locator">Локатор</param>
        /// <param name="path">Путь к файлу</param>
        protected void UpLoadFile(By locator, string path)
        {
            driver.FindElement(locator).SendKeys(Path.GetFullPath(path));
        }
        /// <summary>
        /// Установка значения value, с помощью JS
        /// </summary>
        /// <param name="locator">Локатор</param>
        /// <param name="value">Значение</param>
        /// /// <param name="property">Название параметра</param>
        protected void SetValueFromJS(By locator, string value, string property)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript($"arguments[0].{property.Trim()} = '{value.Trim()}', arguments[0].dispatchEvent(new Event('change'))", driver.FindElement(locator));
        }

        /// <summary>
        /// Ввод текста
        /// </summary>
        /// <param name="locator">Локатор</param>
        /// <param name="text">Текст</param>
        /// <param name="clear">Очистить перед вставкой текста</param>
        protected void InputText(By locator, string text, bool clear = true)
        {
            if (clear)
                driver.FindElement(locator).Clear();
            driver.FindElement(locator).Click();
            driver.FindElement(locator).SendKeys(text);

        }

        /// <summary>
        /// Проверка товара к каталоге
        /// </summary>
        /// <param name="products">Список товаров</param>
        /// <param name="name">Названиетовара, который ищем в каталоге</param>
        /// <returns>Возвращает true, если товар найден.</returns>
        protected bool IsProductInCatalog(IList<IWebElement> products, string name)
        {
            bool result = false;
            foreach (IWebElement product in products)
            {
                if (product.GetAttribute("textContent").Trim() == name.Trim())
                    result = true;
                else
                    continue;
            }
            return result;
        }

        /// <summary>
        /// Переход по URL 
        /// </summary>
        /// <param name="url">URL страницы</param>
        protected void GoToPageURL(string url)
        {
            driver.Navigate().GoToUrl(url);
        }
        /// <summary>
        /// Переход по URL используя локатор
        /// </summary>
        /// <param name="locator">Локатор</param>
        protected void GoToPageURL(By locator)
        {
            IsElementPresent(locator);
            string url = driver.FindElement(locator).GetAttribute("href");
            driver.Navigate().GoToUrl(url);
        }

    }
}
