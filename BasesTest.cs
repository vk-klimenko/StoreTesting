using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
namespace StoreTesting
{
    public class BasesTest
    {
        protected IWebDriver driver;
        protected string baseUrl = "http://litecart/";

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

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
       
     
    
    }
}
