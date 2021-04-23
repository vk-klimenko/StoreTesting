using OpenQA.Selenium;
using System;
using System.Text;

namespace StoreTesting
{
    public class CreateUsers : BasesTest
    {
        private Random rnd;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }

        public CreateUsers(string firstName, string lastName, string address, string password, string city)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            Email = GetRandomEmail();
            Phone = GetPhoneNumFormat();
            Password = password;
            Postcode = GetCodeNumber();
            City = city;
        }

        /// <summary>
        /// Ввод данных при регистрации новых пользователей
        /// </summary>
        public void EnterDataInput(IWebDriver driver, By locator, string str)
        {
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(Keys.Home + str);
        }
        
        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <param name="driver">Драйвер</param>
        /// <param name="locator">Локатор</param>
        public void LogoutAccount(IWebDriver driver, By locator)
        {
            string url = driver.FindElement(locator).GetAttribute("href");
            driver.Navigate().GoToUrl(url);
            
        }
        /// <summary>
        /// Генератор индекса страны
        /// </summary>
        /// <returns>Пятизначный индекс</returns>
        private string GetCodeNumber()
        {
            string code = String.Empty;

            for (int i = 0; i < 5; i++)
            {
                code += rnd.Next(1, 9);
            }
            return code;
        }

        /// <summary>
        /// Генератор рандомной эл. почты
        /// </summary>
        /// <returns>Email</returns>
        private string GetRandomEmail()
        {
            var str = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                str.Append((char)rnd.Next('a', 'z'));
            }
            string email  = str.ToString();
            return $"{email}{rnd.Next(1,100)}@gmail.com";
        }
        /// <summary>
        /// Генератор телефонного номера
        /// </summary>
        /// <returns>Номер в формате "+1 (###) (####)-(##)-(##)"</returns>
        private string GetPhoneNumFormat()
        {
            return  $"+1 {rnd.Next(100, 999)} {rnd.Next(1000, 9999)} {rnd.Next(10, 99)} {rnd.Next(10, 99)}";
        }
    }
}
