using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreTesting
{
    public class CreateProduct : BasesTest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public CreateProduct(string name, string dateFrom, string dateTo)
        {
            Name = name;
            Code = GetCodeProduct();
            Quantity = GetRandomQuantity();
            DateFrom = dateFrom;
            DateTo = dateTo;

        }

        private string GetCodeProduct()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        private string GetRandomDate()
        {
            DateTime date = new DateTime();
            date = DateTime.Now.AddMonths(-1);
            //date.ToShortDateString()

            DateTime.Today.ToShortDateString();
            return null;
        }
        private string GetRandomQuantity()
        {
            return $"{Convert.ToString(new Random().Next(1, 200))}";
        }

    }
}
