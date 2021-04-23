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
    public class ProductBackendTests : BasesTest
    {
        /// <summary>
        /// Задание 10. Проверить, что открывается правильная страница товара
        /// </summary>
        [Test]
        public void LoginAdminPanelMenuSelectTest()
        {
            #region Задание
            /*
             *  Задание 10. Проверить, что открывается правильная страница товара
                Сделайте сценарий, который проверяет, что при клике на товар открывается правильная страница товара в учебном приложении litecart.

                Более точно, нужно открыть главную страницу, выбрать первый товар в блоке Campaigns и проверить следующее:

                а) на главной странице и на странице товара совпадает текст названия товара
                б) на главной странице и на странице товара совпадают цены (обычная и акционная)
                в) обычная цена зачёркнутая и серая (можно считать, что "серый" цвет это такой, у которого в RGBa представлении одинаковые значения для каналов R, G и B)
                г) акционная жирная и красная (можно считать, что "красный" цвет это такой, у которого в RGBa представлении каналы G и B имеют нулевые значения)
                (цвета надо проверить на каждой странице независимо, при этом цвета на разных страницах могут не совпадать)
                д) акционная цена крупнее, чем обычная (это тоже надо проверить на каждой странице независимо)

                Необходимо убедиться, что тесты работают в разных браузерах, желательно проверить во всех трёх ключевых браузерах (Chrome, Firefox, IE).
             */
            #endregion

            AdminPanelAuth("admin", "admin");
            IList<IWebElement> rows = GetElement(By.CssSelector("ul#box-apps-menu")).FindElements(By.CssSelector("li#app-"));

            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Click();
                // проверка элемента h1
                Assert.IsTrue(IsElementPresent(By.CssSelector("td#content h1")));

                // проверка подразделов 
                if (AreElementsPresent(By.CssSelector("ul.docs")))
                {
                    IList<IWebElement> nestedItems = GetElement(By.CssSelector("ul.docs")).FindElements(By.TagName("li"));
                    for (int x = 0; x < nestedItems.Count; x++)
                    {
                        nestedItems[x].Click();
                        // проверка элемента h1
                        Assert.IsTrue(IsElementPresent(By.CssSelector("td#content h1")));
                        nestedItems = GetElement(By.CssSelector("ul.docs")).FindElements(By.TagName("li"));

                    }
                }
                // обновляем элементы
                rows = GetListElements(By.CssSelector("li#app-"));
            }
        }


        /// <summary>
        /// Cценарий добавления товара
        /// </summary>
        [Test]
        public void AddProductTest()
        {
            #region Задание
            /*
             Сделайте сценарий для добавления нового товара(продукта) в учебном приложении litecart(в админке).
             Для добавления товара нужно открыть меню Catalog, в правом верхнем углу нажать кнопку "Add New Product", 
             заполнить поля с информацией о товаре и сохранить.
             Достаточно заполнить только информацию на вкладках General, Information и Prices.Скидки(Campains) на вкладке Prices можно не добавлять.
             Переключение между вкладками происходит не мгновенно,
             поэтому после переключения можно сделать небольшую паузу(о том, как делать более правильные ожидания,
             будет рассказано в следующих занятиях).
             Картинку с изображением товара нужно уложить в репозиторий вместе с кодом.
             При этом указывать в коде полный абсолютный путь к файлу плохо, на другой машине работать не будет. 
             Надо средствами языка программирования преобразовать относительный путь в абсолютный.
             После сохранения товара нужно убедиться, что он появился в каталоге(в админке).Клиентскую часть магазина можно не проверять.
            */
            #endregion
            // 1. Заходим в админку

            AdminPanelAuth("admin", "admin");
            Thread.Sleep(1000);
            // 2. Открываем меню Catalog и нажимаем "Add New Product"

            // click catalog
            PressClick(By.XPath(".//ul[@id='box-apps-menu']//*[.='Catalog']"));
            Thread.Sleep(1000);
            // click add new product
            PressClick(By.XPath(".//td[@id='content']//*[.=' Add New Product']"));

            CreateProduct product = new CreateProduct("Milk");
            // 3.1. Вкладка General Information и Prices и сохраняем. Скидки(Campains) на вкладке Prices можно не добавлять

            // General

            // Status
            PressClick(By.XPath(".//div[@id='tab-general']//label[.=' Enabled']"));

            // Name
            GetElement(By.XPath(".//div[@id='tab-general']//input[@name='name[en]']")).SendKeys(Keys.Home + product.Name);

            // Code
            GetElement(By.XPath(".//div[@id='tab-general']//input[@name='code']")).SendKeys(Keys.Home + product.Code);
            Thread.Sleep(1000);

            // Categories
            PressClick(By.XPath($".//div[@id='tab-general']//input[@name='categories[]' and @value='2']"));

            // Product Groups
            PressClick(By.XPath($".//div[@id='tab-general']//input[@name='product_groups[]' and @value='1-{Convert.ToString(rnd.Next(1, 4))}']"));

            // Quantity
            SetValueFromJS(By.Name("quantity"), product.Quantity, "value");
            Thread.Sleep(1000);
            // Quantity Unit 
            DropDownList(By.XPath(".//select[@name='quantity_unit_id']"), "1");

            // Delivery Status 
            DropDownList(By.XPath(".//select[@name='delivery_status_id']"), "1");
            Thread.Sleep(1000);
            // Sold Out Status   
            DropDownList(By.XPath(".//select[@name='sold_out_status_id']"), Convert.ToString(rnd.Next(1, 3)));

            // Upload Images
            UpLoadFile(By.XPath(".//input[@name='new_images[]']"), @"\icon.png");

            // Date from
            SetValueFromJS(By.Name("date_valid_from"), product.DateFrom, "value");

            // Date to
            SetValueFromJS(By.Name("date_valid_to"), product.DateTo, "value");

            // Переход на вкладку Information
            PressClick(By.XPath(".//ul[@class='index']/li/a[.='Information']"));
            Thread.Sleep(1000);

            // 3.2. Вкладка Information

            // Manufacturer
            DropDownList(By.XPath(".//div[@id='tab-information']//select[@name='manufacturer_id']"), "1");

            // Keywords
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='keywords']"), "AAA, BBB, CCC, DDD, EEE");

            // Short Description
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='short_description[en]']"), "This is a very necessary thing");

            // Description     
            InputText(By.XPath(".//div[@class='trumbowyg-editor']"), @"Lorem Ipsum is simply dummy text of the printing and typesetting industry.Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.");

            // Head Title
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='head_title[en]']"), "Head title");

            // Meta Description
            InputText(By.XPath(".//div[@id='tab-information']//input[@name='meta_description[en]']"), "This is meta description");

            // Переход на вкладку Prices
            PressClick(By.XPath(".//ul[@class='index']/li/a[.='Prices']"));
            Thread.Sleep(1000);

            // 3.3. Вкладка Prices

            // Purchase Price
            SetValueFromJS(By.Name("purchase_price"), product.Price, "value");

            // Select currency
            DropDownList(By.XPath(".//select[@name='purchase_price_currency_code']"), "EUR");

            // Price	Price Incl. Tax (?)
            SetValueFromJS(By.Name("prices[USD]"), Convert.ToString(rnd.Next(1, 500)), "value");
            SetValueFromJS(By.Name("prices[EUR]"), Convert.ToString(rnd.Next(1, 500)), "value");

            // Campaigns
            PressClick(By.Id("add-campaign"));

            SetValueFromJS(By.Name("campaigns[new_1][start_date]"), $"{product.DateFrom}T12:00", "value");
            SetValueFromJS(By.Name("campaigns[new_1][end_date]"), $"{product.DateTo}T23:00", "value");
            SetValueFromJS(By.Name("campaigns[new_1][percentage]"), Convert.ToString(rnd.Next(10, 50)), "value");
            SetValueFromJS(By.Name("campaigns[new_1][USD]"), Convert.ToString(rnd.Next(10, 500)), "value");
            SetValueFromJS(By.Name("campaigns[new_1][EUR]"), Convert.ToString(rnd.Next(10, 400)), "value");

            // Button Save
            PressClick(By.XPath(".//span[@class='button-set']/button[@name='save']"));
            Thread.Sleep(1000);

            // 4. Проверка, что товар добавлен
            // click catalog
            PressClick(By.XPath(".//ul[@id='box-apps-menu']//*[.='Catalog']"));
            Thread.Sleep(1000);

            AreElementsPresent(By.XPath(".//td[@id='content']//table[@class='dataTable']//tr[@class='row' and position()>2]//a"));
            IList<IWebElement> productList = GetListElements(By.XPath(".//td[@id='content']//table[@class='dataTable']//tr[@class='row' and position()>2]//a"));

            Assert.IsTrue(IsProductInCatalog(productList, product.Name));
        }


    }
}
