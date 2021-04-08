using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

namespace StoreTesting
{
    [TestFixture]
    public class AdminPanelTest : BasesTest
    {
        /// <summary>
        /// Авторизация в админ панель
        /// </summary>
        [Test]
        public void LoginAdminPanel()
        {
            AdminPanelAuth("admin", "admin");
        }

        /// <summary>
        /// Авторизация в админ панель и чекбокс запомнить пользователя
        /// </summary>
        [Test]
        public void LoginRememberAdminPanel()
        {
            AdminPanelAuth("admin", "admin", true);
        }

        /// <summary>
        /// Выход из админ панели
        /// </summary>
        [Test]
        public void LogOutAdminPanel()
        {
            AdminPanelLogout("admin", "admin");
        }

        /// <summary>
        /// Клик по левому сайдбару и проверка H1
        /// </summary>
        [Test]
        public void LoginAdminPanelMenuSelect()
        {
            AdminPanelAuth("admin", "admin"); 
            IList<IWebElement> rows = driver.FindElement(By.CssSelector("ul#box-apps-menu")).FindElements(By.CssSelector("li#app-"));

            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Click();
                // проверка элемента h1
                Assert.IsTrue(IsElementPresent(By.CssSelector("td#content h1")));

                // проверка подразделов 
                if (AreElementsPresent(By.CssSelector("ul.docs")))
                {
                    IList<IWebElement> nestedItems = driver.FindElement(By.CssSelector("ul.docs")).FindElements(By.TagName("li"));
                    for (int x = 0; x < nestedItems.Count; x++)
                    {
                        nestedItems[x].Click();
                        // проверка элемента h1
                        Assert.IsTrue(IsElementPresent(By.CssSelector("td#content h1")));
                        nestedItems = driver.FindElement(By.CssSelector("ul.docs")).FindElements(By.TagName("li"));
                        
                    }
                }
                // обновляем элементы
                rows = driver.FindElements(By.CssSelector("li#app-"));
                
            }
        }

    }
}
