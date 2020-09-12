using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoreApp.AddUpProduct
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentView
    {
        public AddProduct()
        {
            InitializeComponent();
        }
        public AddProduct(Product product)
        {

        }
    }
}