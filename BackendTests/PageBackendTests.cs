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
    public class PageBackendTests : BasesTest
    {
        /// <summary>
        /// Проверка, что ссылки открываются в новом окне
        /// </summary>
        [Test]
        public void CheckOpenNewWindowTest()
        {
            #region Задание
            /*
            Сделайте сценарий, который проверяет, что ссылки на странице редактирования страны открываются в новом окне.
            Сценарий должен состоять из следующих частей:

            1. зайти в админку
            2. открыть пункт меню Countries(или страницу http://localhost/litecart/admin/?app=countries&doc=countries)
            3. открыть на редактирование какую-нибудь страну или начать создание новой
            4. возле некоторых полей есть ссылки с иконкой в виде квадратика со стрелкой --они ведут на внешние страницы и открываются в новом окне, именно это и нужно проверить.

            Конечно, можно просто убедиться в том, что у ссылки есть атрибут target = "_blank".Но в этом упражнении требуется именно кликнуть по ссылке, 
            чтобы она открылась в новом окне, потом переключиться в новое окно, закрыть его, вернуться обратно, и повторить эти действия для всех таких ссылок.
            
            Не забудьте, что новое окно открывается не мгновенно, поэтому требуется ожидание открытия окна.
            */
            #endregion

            AdminPanelAuth("admin", "admin");

            PressClick(By.XPath(".//ul[@id='box-apps-menu']//span[@class='name' and text()='Countries']"));

            GoToPageURL("http://litecart/admin/?app=countries&doc=edit_country&country_code=CU");

            string mainWindowId = driver.CurrentWindowHandle;
            IList<string> existWindows = driver.WindowHandles;

            IList<IWebElement> listUrls = GetListElements(By.CssSelector("#content a:nth-child(n+2)[target='_blank']"));
            Assert.IsTrue(AreElementsPresent(By.CssSelector("#content a:nth-child(n+2)[target='_blank']")));

            foreach (IWebElement el in listUrls)
            {
                string url = el.GetAttribute("href").Trim();
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.open()");

                string newWindow = wait.Until<string>(driver => TryFindNewWindow(existWindows));

                driver.SwitchTo().Window(newWindow);
                Assert.IsTrue(driver.CurrentWindowHandle == newWindow);
                GoToPageURL(url);

                driver.Close();

                driver.SwitchTo().Window(mainWindowId);
            }

        }

        public string TryFindNewWindow(IList<string> existWindows)
        {
            IList<string> newHandles = driver.WindowHandles.Except(existWindows).ToList();
            //newHandles.Except(existWindows).ToList();
            return newHandles.Count > 0 ? newHandles[0] : null;
        }


        
       
    }
}
