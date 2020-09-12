using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Models
{
    public class SingleSale
    {
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public List<Product> ItesmsSold { get; set; }
        public Custommer Customer { get; set; }
        public DateTime date { get; set; }
    }
}
