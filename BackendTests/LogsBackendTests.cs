using System;
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

            AdminPanelAuth("admin", "admin");

            GoToPageURL("http://litecart/admin/?app=catalog&doc=catalog&category_id=1");

            List<string> ListLogs = new List<string>();

            IList<IWebElement> products = GetListElements(By.XPath(".//td[@id='content']//tr[@class='row' and position() > 4]//a[not(@title='Edit')]"));

            Assert.IsTrue(AreElementsPresent(By.XPath(".//td[@id='content']//tr[@class='row' and position() > 4]//a[not(@title='Edit')]")));

            foreach (IWebElement item in products)
            {
                string url = item.GetAttribute("href").Trim();
                
                NavigateOpenNewWindow();

                string mainWindowId = driver.CurrentWindowHandle;
                string newWindowID = driver.WindowHandles.Last();

                NavigateSwitchToWindow(newWindowID);
                
                GoToPageURL(url);

                /*  ----- Проверить логи на странице ------- */
                ICollection<LogEntry> LogBrowsers = driver.Manage().Logs.GetLog("browser");
                if (LogBrowsers.Count == 0)
                {
                    CloseWindow();
                    NavigateSwitchToWindow(mainWindowId);
                    continue;
                }
                
                ListLogs = GetBrowserLogs(LogBrowsers, 
                                          ListLogs, 
                                          $"Продукт: {driver.Title.Split(':')[1].Trim()}{Environment.NewLine}Ссылка: {driver.Url}"
                                          );

                CloseWindow();
                NavigateSwitchToWindow(mainWindowId);
            }


            foreach (string item in ListLogs)
            {
                Console.Out.WriteLine(item);
            }

        }


        /// <summary>
        /// Получить записи логов страницы товара
        /// </summary>
        /// <param name="logBrowsers">Коллекция LogEntry</param>
        /// <param name="listLogs">Коллекция логов</param>
        /// <param name="title">Заголовок</param>
        /// <returns>Возвращает строковую коллекцию логов</returns>
        private List<string> GetBrowserLogs(ICollection<LogEntry> logBrowsers, List<string> listLogs, string title)
        {
            listLogs.Add(title);
            
            foreach (LogEntry log in logBrowsers)
            {
                listLogs.Add($"Дата и время: {log.Timestamp}");
                listLogs.Add($"Уровень: {log.Level}");
                listLogs.Add($"Сообщение: {log.Message}");
            }
            listLogs.Add(new string('-', 150));
            return listLogs;
        }
    }
}
