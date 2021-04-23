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
    public class UserFrontendTests : BasesTest
    {
        /// <summary>
        /// Сценарий регистрации новых пользователей
        /// </summary>
        [Test]
        public void UserRegistrationTest()
        {
            #region Задание
            // Сценарий должен состоять из следующих частей:
            //1) регистрация новой учётной записи с достаточно уникальным адресом электронной почты(чтобы не конфликтовало с ранее созданными пользователями, в том числе при предыдущих запусках того же самого сценария),
            //2) выход(logout), потому что после успешной регистрации автоматически происходит вход,
            //3) повторный вход в только что созданную учётную запись,
            //4) и ещё раз выход.
            //В качестве страны выбирайте United States, штат произвольный.При этом формат индекса --пять цифр.
            //Можно оформить сценарий либо как тест, либо как отдельный исполняемый файл.
            //Проверки можно никакие не делать, только действия - заполнение полей, нажатия на кнопки и ссылки.Если сценарий дошёл до конца, то есть созданный пользователь смог выполнить вход и выход-- значит создание прошло успешно.
            #endregion
            GoToPageURL($"{baseUrl}en/create_account");
            CreateUsers user = new CreateUsers("Masha", "Sasha", "Mankheton 77", "password2", "Las-Vegas");

            // 1. Запонение формы
            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[2]/td[1]/input[@name='firstname']"), user.FirstName);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[2]/td[1]/input[@name='firstname']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[2]/td[2]/input[@name='lastname']"), user.LastName);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[2]/td[2]/input[@name='lastname']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[3]/td[1]/input[@name='address1']"), user.Address);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[3]/td[1]/input[@name='address1']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[4]/td[1]/input[@name='postcode']"), user.Postcode);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[4]/td[1]/input[@name='postcode']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[4]/td[2]/input[@name='city']"), user.City);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[4]/td[2]/input[@name='city']")).GetAttribute("value"));

            // ---------------------------------------------------------------------------------------------------
            // Клик по элементу выпадающего списка
            PressClick(By.CssSelector("span [role='presentation']"));
            // В откр. списке ищем поле ввода
            driver.FindElement(By.CssSelector("input.select2-search__field")).SendKeys("United States" + Keys.Enter);
            // ---------------------------------------------------------------------------------------------------

            DropDownList(By.CssSelector("select[name='zone_code']"), rnd.Next(1, 65));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[6]/td[1]/input[@name='email']"), user.Email);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[6]/td[1]/input[@name='email']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[6]/td[2]/input[@name='phone']"), user.Phone);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[6]/td[2]/input[@name='phone']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[8]/td[1]/input[@name='password']"), user.Password);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[1]/input[@name='password']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//div[@id='create-account']//tr[8]/td[2]/input[@name='confirmed_password']"), user.Password);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[2]/input[@name='confirmed_password']")).GetAttribute("value"));

            //driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[1]/input[@name='password']")).SendKeys(Keys.Control + "c" + user.Password);
            //driver.FindElement(By.XPath(".//div[@id='create-account']//tr[8]/td[2]/input[@name='confirmed_password']")).SendKeys(Keys.Control + "v");

            PressClick(By.XPath(".//div[@id='create-account']//tr[9]//button[@name='create_account']"));

            // 2. Logout из аккаунта

            user.LogoutAccount(driver, By.XPath(".//div[@id='box-account']//li[last()]/a[.='Logout']"));
            Assert.IsTrue(driver.FindElement(By.XPath(".//div[@id='box-account-login']/h3[@class='title']")).GetAttribute("textContent") == "Login");

            // 3. Авторизация под пользователем

            user.EnterDataInput(driver, By.XPath(".//form[@name='login_form']//input[@name='email']"), user.Email);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//form[@name='login_form']//input[@name='email']")).GetAttribute("value"));

            user.EnterDataInput(driver, By.XPath(".//form[@name='login_form']//input[@name='password']"), user.Password);
            Assert.IsNotNull(driver.FindElement(By.XPath(".//form[@name='login_form']//input[@name='password']")).GetAttribute("value"));

            PressClick(By.XPath(".//form[@name='login_form']//button[@name='login']"));

            // Проверяем, что зашли под пользователем
            Assert.IsTrue(driver.FindElement(By.XPath(".//div[@id='box-account']/h3[@class='title']")).GetAttribute("textContent") == "Account");

            // 4. Logout из аккаунта

            user.LogoutAccount(driver, By.XPath(".//div[@id='box-account']//li[last()]/a[.='Logout']"));

            // Проверяем, что вышли под пользователем
            Assert.IsTrue(driver.FindElement(By.XPath(".//div[@id='box-account-login']/h3[@class='title']")).GetAttribute("textContent") == "Login");
        }
    }
}
