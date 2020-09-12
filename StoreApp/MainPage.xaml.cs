using Newtonsoft.Json;
using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StoreApp
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Product> products;
        ObservableCollection<Product> BillItems;
        ObservableCollection<Product> SearchItems;
        Product SelectedItem;
        Double Sum = 0;
        Double Total = 0;
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

            for(int i = 0; i < 3000; i++)
            {
                products.Add(new Product()
                {
                    Name = "Test4" + i,
                    Price = 199.9,
                    Quantity = 10,
                    SNo = 1
                });
            }



            BillItems = new ObservableCollection<Product>() { };
            SearchItems = new ObservableCollection<Product>() { };
            UpdateSearchData();
            MainCollectionView.ItemsSource = SearchItems;
            BoughtCollectionView.ItemsSource = BillItems;
            CustommersCollectionView.ItemsSource = products;
        }

        public void UpdateSearchData()
        {
            SearchItems.Clear();
            for (int i = 0; i < products.Count; i++)
            {
                SearchItems.Add(products[i]);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button but = sender as Button;
            Product Rem = but.BindingContext as Product;
            Product Old = products.Where(x => x.Name == Rem.Name).FirstOrDefault();
            BillItems.Remove(Rem);
            products.Remove(Old);
            products.Insert(0, new Product()
            {
                Name = Old.Name,
                Quantity = Old.Quantity + Rem.Quantity,
                Price = Old.Price
            });
            UpdateSearchData();
        }

        private void AddCancelClicked(object sender, EventArgs e)
        {
            if(AddProductView.IsVisible == true)
                AddProductView.IsVisible = false;

            else
                AddProductView.IsVisible = true;
        }

        private void AddClicked(object sender, EventArgs e)
        {
            try
            {
                if (NewProductNameEntry.Text != "" && NewProductPriceEntry.Text != "" && NewProductQuantityEntry.Text != "")
                {
                    Product product = new Product()
                    {
                        Name = NewProductNameEntry.Text,
                        Price = Convert.ToDouble(NewProductPriceEntry.Text),
                        Quantity = Convert.ToInt32(NewProductQuantityEntry.Text),
                        SNo = products.Count
                    };
                    products.Add(product);
                    DisplayAlert("Success", "The Product was successfully Added", "ok");

                    string FilePath = Path.Combine(FileSystem.AppDataDirectory, "ProductsData.json");
                    string data = JsonConvert.SerializeObject(products);
                    App.Current.Properties["Products"] = data;
                    File.WriteAllText(FilePath, data);
                }
                else
                {
                    DisplayAlert("Error", "Please Enter the Name, Price and Quantity", "ok");
                }
            }
            catch(Exception m)
            {
                DisplayAlert("Error", "Input is not in correct format", "ok");
            }
            
        }










        private void AddToBill_Clicked(object sender, EventArgs e)
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.Quantity >= Convert.ToInt32(QuantityEntry.Text) && SelectedItem.Quantity != 0)
                {
                    Product product = new Product()
                    {
                        Name = SelectedItem.Name,
                        Price = SelectedItem.Price,
                        Quantity = Convert.ToInt32(QuantityEntry.Text)
                    };
                    BillItems.Add(product);
                    int Quantity = SelectedItem.Quantity - product.Quantity;
                    products.Remove(SelectedItem);

                    products.Insert(0, new Product()
                    {
                        Name = SelectedItem.Name,
                        Price = SelectedItem.Price,
                        Quantity = Quantity
                    });
                    SelectedItem = null;
                    UpdateEntry();
                    UpdateSearchData();
                }
                else
                    DisplayAlert("Not enough items", "There are not items left in stock. Choose a quanity lesser than or equal to that in stock", "Ok");
            }
            
        }
        public void UpdateEntry()
        {
            QuantityEntry.Text = "";
            ProductEntry.Text = "";
            PriceEntry.Text = "";
        }
        public void UpdateTotal()
        {
            try
            {
                for(int i = 0; i < BillItems.Count; i++)
                {
                    Sum = Sum + BillItems[i].Price * BillItems[i].Quantity;
                }

                try
                {
                    Total = Sum - Convert.ToDouble(DiscountEntry.Text);
                }
                catch(Exception j)
                {
                    Total = Sum;
                }

                SumLabel.Text = Sum.ToString();
                TotalLabel.Text = Total.ToString();
                

            }
            catch(Exception g)
            {

            }
        }
        private void MainCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MainCollectionView.SelectedItem != null)
            {
                SelectedItem = e.CurrentSelection[0] as Product;
                ProductEntry.Text = SelectedItem.Name;
                PriceEntry.Text = SelectedItem.Price.ToString();
                QuantityEntry.Text = SelectedItem.Quantity.ToString();
                
            }
            MainCollectionView.SelectedItem = null;

        }

        private void ProductEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchItems.Clear();
            for(int i = 0; i<products.Count; i++)
            {
                if (products[i].Name.Contains(e.NewTextValue))
                {
                    SearchItems.Add(products[i]);
                }
            }
        }
    }
}
