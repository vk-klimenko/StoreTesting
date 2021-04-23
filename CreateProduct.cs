using System;

namespace StoreTesting
{
    public class CreateProduct : BasesTest
    {
        private Random rnd;
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
            return rnd.Next(1000, 9999).ToString();
        }

        private string GetRandomDate()
        {
            string dtFrom = DateTime.Now.ToString("yyyy-MM-dd");
            string dtTo = DateTime.Now.AddMonths(+ rnd.Next(1,7)).ToString("yyyy-MM-dd");
            return $"{dtFrom}:{dtTo}";
        }
        private string GetRandomNumber()
        {
            return $"{Convert.ToString(rnd.Next(1, 200))}";
        }

    }
}
