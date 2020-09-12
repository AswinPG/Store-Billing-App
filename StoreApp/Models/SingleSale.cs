using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StoreApp.Models
{
    public class SingleSale
    {
        public double TotalAmount { get; set; }
        public string Discount { get; set; }
        public ObservableCollection<Product> ItesmsSold { get; set; }
        public Custommer Customer { get; set; }
        public DateTime date { get; set; }
    }
}
