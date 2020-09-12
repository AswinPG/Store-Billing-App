using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StoreApp
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Product> products;
        public MainPage()
        {
            InitializeComponent();
            products = new ObservableCollection<Product>()
            {
                new Product()
                {
                    Name = "Test",
                    Price = 199.9,
                    Quantity = 10,
                    SNo = 1
                },
                new Product()
                {
                    Name = "Test1",
                    Price = 199.9,
                    Quantity = 10,
                    SNo = 1
                },
                new Product()
                {
                    Name = "Test2",
                    Price = 199.9,
                    Quantity = 10,
                    SNo = 1
                },
                new Product()
                {
                    Name = "Test3",
                    Price = 199.9,
                    Quantity = 10,
                    SNo = 1
                },
                new Product()
                {
                    Name = "Test5",
                    Price = 199.9,
                    Quantity = 10,
                    SNo = 1
                },
                new Product()
                {
                    Name = "Test4",
                    Price = 199.9,
                    Quantity = 10,
                    SNo = 1
                }
            };
            MainCollectionView.ItemsSource = products;
            BoughtCollectionView.ItemsSource = products;
            CustommersCollectionView.ItemsSource = products;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button but = sender as Button;
            Product Rem = but.BindingContext as Product;
            products.Remove(Rem);
        }
    }
}
