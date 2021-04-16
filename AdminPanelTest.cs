using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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


        /// <summary>
        /// Проверка сортировки стран
        /// </summary>
        [Test]
        public void CheckingSortCountries()
        {
            AdminPanelAuth("admin", "admin");

            driver.Navigate().GoToUrl($"{baseUrl}admin/?app=countries&doc=countries");

            // "tr.row td:nth-child(5n) a"

            string locator = "tr.row";

            IList<IWebElement> rows = driver.FindElements(By.CssSelector(locator));
            
            // 
            Assert.IsTrue(AreElementsPresent(By.CssSelector(locator)));

            // Проверка
            Assert.IsTrue(CheckingCountry(rows));

        }

        /// <summary>
        /// Проверка сортировки геозон
        /// </summary>
        [Test]
        public void CheckingSortGeo()
        {
            AdminPanelAuth("admin", "admin");

            driver.Navigate().GoToUrl($"{baseUrl}admin/?app=geo_zones&doc=geo_zones");

            IList<IWebElement> geoZone = driver.FindElements(By.CssSelector("[name = 'geo_zones_form'] tr.row"));

            for (int i = 0; i < geoZone.Count; i++)
            {

                driver.Url = geoZone[i].FindElement(By.CssSelector("tr.row td:nth-child(3n) a")).GetAttribute("href").Trim();

                IList<IWebElement> rowsGeoZone = driver.FindElements(By.XPath(".//table[@id='table-zones']//tr[position() > 1 and position() < last()]//select[contains(@name,'zone_code')]/option"));
                Assert.IsTrue(AreElementsPresent(By.XPath(".//table[@id='table-zones']//tr[position() > 1 and position() < last()]//select[contains(@name,'zone_code')]/option")));

                List<string> geoList = new List<string>();
                foreach(IWebElement row in rowsGeoZone)
                {
                    if (row.GetAttribute("selected") == "true")
                        geoList.Add(row.GetAttribute("innerText").Trim());
                }

                List<string> geoListSort = geoList;
                geoListSort.Sort();
                Assert.IsTrue(geoList.SequenceEqual(geoListSort));


                driver.Navigate().Back();
                
                geoList.Clear();
                geoListSort.Clear();

                geoZone = driver.FindElements(By.CssSelector("[name = 'geo_zones_form'] tr.row"));
            }

        }

        /// <summary>
        /// Cценарий добавления товара
        /// </summary>
        [Test]
        public void AddProduct()
        {
            // Сделайте сценарий для добавления нового товара(продукта) в учебном приложении litecart(в админке).
            // Для добавления товара нужно открыть меню Catalog, в правом верхнем углу нажать кнопку "Add New Product", 
            // заполнить поля с информацией о товаре и сохранить.
            // Достаточно заполнить только информацию на вкладках General, Information и Prices.Скидки(Campains) на вкладке Prices можно не добавлять.
            // Переключение между вкладками происходит не мгновенно, 
            // поэтому после переключения можно сделать небольшую паузу(о том, как делать более правильные ожидания, 
            // будет рассказано в следующих занятиях).
            // Картинку с изображением товара нужно уложить в репозиторий вместе с кодом.
            // При этом указывать в коде полный абсолютный путь к файлу плохо, на другой машине работать не будет. 
            // Надо средствами языка программирования преобразовать относительный путь в абсолютный.
            // После сохранения товара нужно убедиться, что он появился в каталоге(в админке).Клиентскую часть магазина можно не проверять.

            // 1. Заходим в админку

            AdminPanelAuth("admin", "admin");
            Thread.Sleep(3000);
            // 2. Открываем меню Catalog и нажимаем "Add New Product"

            // click catalog
            driver.FindElement(By.XPath(".//ul[@id='box-apps-menu']//*[.='Catalog']")).Click();
            Thread.Sleep(3000);

            // click add new product
            driver.FindElement(By.XPath(".//td[@id='content']//*[.=' Add New Product']")).Click();
            Thread.Sleep(3000);

            CreateProduct product = new CreateProduct("Ball");

            // 3.1. Вкладка General Information и Prices и сохраняем. Скидки(Campains) на вкладке Prices можно не добавлять

            // General

            Thread.Sleep(3000);
            // Status
            driver.FindElement(By.XPath(".//div[@id='tab-general']//label[.=' Enabled']")).Click();

            // Name
            driver.FindElement(By.XPath(".//div[@id='tab-general']//input[@name='name[en]']")).SendKeys(Keys.Home + product.Name);

            // Code
            driver.FindElement(By.XPath(".//div[@id='tab-general']//input[@name='code']")).SendKeys(Keys.Home + product.Code);

            Thread.Sleep(3000);

            // Categories
            driver.FindElement(By.XPath($".//div[@id='tab-general']//input[@name='categories[]' and @value='2']")).Click();

            // Product Groups
            driver.FindElement(By.XPath($".//div[@id='tab-general']//input[@name='product_groups[]' and @value='1-{Convert.ToString(new Random().Next(1, 4))}']")).Click();

            // Quantity
            SetValueFromJS(By.Name("quantity"), product.Quantity, "value");

            Thread.Sleep(3000);

            // Quantity Unit <select name="quantity_unit_id" data-size="auto"> SelectElementByValue=1
            DropDownList(By.XPath(".//select[@name='quantity_unit_id']"), "1");

            Thread.Sleep(3000);

            // Delivery Status 
            DropDownList(By.XPath(".//select[@name='delivery_status_id']"), "1");
            
            // Sold Out Status   
            DropDownList(By.XPath(".//select[@name='sold_out_status_id']"), Convert.ToString(new Random().Next(1,3)));
            
            // Upload Images
            UpLoadFile(By.XPath(".//input[@name='new_images[]']"), @"C:\icon.png");

            // Date from
            SetValueFromJS(By.Name("date_valid_from"), product.DateFrom, "value");

            // Date to
            SetValueFromJS(By.Name("date_valid_to"), product.DateTo, "value");

            Thread.Sleep(3000);

            // Переход на вкладку Information
            PressButton(By.XPath(".//ul[@class='index']/li/a[.='Information']"));
            Thread.Sleep(3000);

            // 3.2. Вкладка Information

            // Manufacturer
            DropDownList(By.XPath(".//div[@id='tab-information']//select[@name='manufacturer_id']"), "1");

            // Keywords
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='keywords']"), "AAA, BBB, CCC, DDD, EEE");

            // Short Description
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='short_description[en]']"), "This is a very necessary thing");

            // Description
            InputText(By.XPath(".//div[@id='tab-information']//textarea[@name='description[en]']"), 
                        @"Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
                        Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, 
                        when an unknown printer took a galley of type and scrambled it to make a type specimen book. 
                        It has survived not only five centuries, but also the leap into electronic typesetting, 
                        remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset 
                        sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like 
                        Aldus PageMaker including versions of Lorem Ipsum."
                        );

            // Head Title
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='head_title[en]']"), "Rubber ball");

            // Meta Description
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='meta_description[en]']"), "This is meta description");

            // Переход на вкладку Prices
            PressButton(By.XPath(".//ul[@class='index']/li/a[.='Prices']"));
            Thread.Sleep(3000);

            // 3.3. Вкладка Prices

            // Purchase Price
            SetValueFromJS(By.Name("purchase_price"), product.Price, "value");
            Thread.Sleep(3000);

            // Select currency
            DropDownList(By.XPath(".//select[@name='purchase_price_currency_code']"), "EUR");
            Thread.Sleep(3000);

            // Price	Price Incl. Tax (?)
            SetValueFromJS(By.Name("prices[USD]"), Convert.ToString(new Random().Next(1, 500)), "value");
            SetValueFromJS(By.Name("prices[EUR]"), Convert.ToString(new Random().Next(1, 500)), "value");

            // Button Save
            PressButton(By.XPath(".//span[@class='button-set']/button[@name='save']"));
            Thread.Sleep(3000);


            // 4. Проверка, что товар добавлен
            // click catalog
            driver.FindElement(By.XPath(".//ul[@id='box-apps-menu']//*[.='Catalog']")).Click();
            Thread.Sleep(3000);
            
            AreElementsPresent(By.XPath(".//td[@id='content']//table[@class='dataTable']//tr[@class='row' and position()>2]//a"));
            IList<IWebElement> productList = driver.FindElements(By.XPath(".//td[@id='content']//table[@class='dataTable']//tr[@class='row' and position()>2]//a"));
            
            Assert.IsTrue(IsProductInCatalog(productList, product.Name));
        }

    }
}
