using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

namespace StoreTesting.FrontendTests
{
    public class LoginTests : BasesTest
    {
        /// <summary>
        /// Авторизация в админ панель
        /// </summary>
        [Test]
        public void LoginAdminPanelTest()
        {
            AdminPanelAuth("admin", "admin");
        }

        /// <summary>
        /// Авторизация в админ панель и чекбокс запомнить пользователя
        /// </summary>
        [Test]
        public void LoginRememberAdminPanelTest()
        {
            AdminPanelAuth("admin", "admin", true);
        }

        /// <summary>
        /// Выход из админ панели
        /// </summary>
        [Test]
        public void LogOutAdminPanelTest()
        {
            AdminPanelLogout("admin", "admin");
        }
    }
}
