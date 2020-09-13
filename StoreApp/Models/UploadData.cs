using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StoreApp.Models
{
    public class UploadData
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Custommer> Custommers { get; set; }
        public ObservableCollection<SingleSale> Sales { get; set; }
        public DateTime Datetime { get; set; }
    }
}
