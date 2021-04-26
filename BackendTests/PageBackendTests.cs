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

            GoToPageURL("http://litecart/admin/?app=countries&doc=edit_country&country_code=AR");

            string mainWindowId = driver.CurrentWindowHandle;
            IList<string> existWindows = driver.WindowHandles;

            IList<IWebElement> listUrls = GetListElements(By.CssSelector("#content a:nth-child(n+2)[target='_blank']"));
            Assert.IsTrue(AreElementsPresent(By.CssSelector("#content a:nth-child(n+2)[target='_blank']")));

            foreach (IWebElement el in listUrls)
            {
                string url = el.GetAttribute("href").Trim();
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.open()");

                // Ожидание появления нового окна
                string newWindow = wait.Until<string>((driver) =>
                {
                    string foundWindow = null;
                    IList<string> newHandles = driver.WindowHandles.Except(existWindows).ToList();
                    //if (newHandles.Count > 0)
                    //    foundWindow = newHandles[0];
                    wait.Until<bool>(dr => HANDLES(newHandles));
                    return newHandles[0];
                    //return foundWindow;
                });

                

                //string newWindow = wait.Until<string>(driver => TryFindNewWindow(driver, existWindows));


                driver.SwitchTo().Window(newWindow);
                Assert.IsTrue(driver.CurrentWindowHandle == newWindow);
                GoToPageURL(url);

                driver.Close();

                driver.SwitchTo().Window(mainWindowId);
            }

        }

        private string TryFindNewWindow(IWebDriver drv, IList<string> existWindows)
        {
            IList<string> newHandles = drv.WindowHandles.Except(existWindows).ToList();
            if (newHandles.Count > 0)
                return newHandles[0];
            return null;

        }
        private bool HANDLES(IList<string> handle)
        {
            if (handle.Count > 0)
                return true;
            return false;

        }

        //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        //        int previousWinCount = driver.WindowHandles.Count;
        //        // Perform the action to open a new Window
        //        wait.Until(driver => driver.WindowHandles.Count == (previousWinCount + 1));
        //        driver.SwitchTo().Window(driver.WindowHandles.Last());
        //        wait.Until(driver => driver.Url.Contains("desired_url_or_a_substring_of_it"));
        /* -------------------------------------------------------------------------------------------*/

        //driver.SwitchTo().Window(driver.WindowHandles.Last());

        /* -------------------------------------------------------------------------------------------*/
        //def wait_for_new_window(driver, timeout= 10): #http://stackoverflow.com/questions/26641779/python-selenium-how-to-wait-for-new-window-opens
        //handles_before = driver.window_handles
        //yield
        //WebDriverWait(driver, timeout).until(
        //    lambda driver: len(handles_before) != len(driver.window_handles))
        /* -------------------------------------------------------------------------------------------*/
    }
}
