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
        public string Price { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        

        public CreateProduct(string name)
        {
            Name = name;
            Code = GetCodeProduct();
            Quantity = GetRandomNumber();
            Price = GetRandomNumber();
            string date = GetRandomDate();
            DateFrom = date.Split(':')[0];
            DateTo = date.Split(':')[1];
        }
        
        private string GetCodeProduct()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        private string GetRandomDate()
        {
            string dtFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dtTo = DateTime.Now.AddMonths(+ new Random().Next(1,7)).ToString("yyyy-MM-dd");
            return $"{dtFrom}:{dtTo}";
        }
        private string GetRandomNumber()
        {
            return $"{Convert.ToString(new Random().Next(1, 200))}";
        }

    }
}
