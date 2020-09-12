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
        ObservableCollection<Custommer> Custommers;
        ObservableCollection<Custommer> SearchCustommers;
        ObservableCollection<SingleSale> SaleData;
        Product SelectedItem;
        Custommer SelectedCustommer;
        Double Sum = 0;
        Double Total = 0;
        public MainPage()
        {
            InitializeComponent();
            products = new ObservableCollection<Product>()
            {
                //new Product()
                //{
                //    Name = "Test",
                //    Price = 199.9,
                //    Quantity = 10,
                //    SNo = 1
                //},
                //new Product()
                //{
                //    Name = "Test1",
                //    Price = 199.9,
                //    Quantity = 10,
                //    SNo = 1
                //},
                //new Product()
                //{
                //    Name = "Test2",
                //    Price = 199.9,
                //    Quantity = 10,
                //    SNo = 1
                //},
                //new Product()
                //{
                //    Name = "Test3",
                //    Price = 199.9,
                //    Quantity = 10,
                //    SNo = 1
                //},
                //new Product()
                //{
                //    Name = "Test5",
                //    Price = 199.9,
                //    Quantity = 10,
                //    SNo = 1
                //},
                //new Product()
                //{
                //    Name = "Test4",
                //    Price = 199.9,
                //    Quantity = 10,
                //    SNo = 1
                //}
            };

            //for(int i = 0; i < 3000; i++)
            //{
            //    products.Add(new Product()
            //    {
            //        Name = "Test4" + i,
            //        Price = 199.9,
            //        Quantity = 10,
            //        SNo = 1
            //    });
            //}

            Custommers = new ObservableCollection<Custommer>() { };
            SaleData = new ObservableCollection<SingleSale>() { };
            BillItems = new ObservableCollection<Product>() { };
            SearchItems = new ObservableCollection<Product>() { };
            SearchCustommers = new ObservableCollection<Custommer>() { };
            UpdateSearchData();
            MainCollectionView.ItemsSource = SearchItems;
            BoughtCollectionView.ItemsSource = BillItems;
            CustommersCollectionView.ItemsSource = SearchCustommers;
            UpdateCollectionView.ItemsSource = products;
            GetLocalData();
        }



        public void GetLocalData()
        {
            try
            {
                string custommers = App.Current.Properties["Custommers"].ToString();
                Custommers = JsonConvert.DeserializeObject<ObservableCollection<Custommer>>(custommers);
                string items = App.Current.Properties["Products"].ToString();
                products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(items);
                string sales = App.Current.Properties["Sales"].ToString();
                SaleData = JsonConvert.DeserializeObject<ObservableCollection<SingleSale>>(sales);
                UpdateSearchData();
                UpdateCustommerData();
            }
            catch (Exception f)
            {
                DisplayAlert("Error", f.Message, "Ok");
            }

        }




        public void UpdateSearchData()
        {
            SearchItems.Clear();
            for (int i = 0; i < products.Count; i++)
            {
                SearchItems.Add(products[i]);
            }
        }
        public void UpdateCustommerData()
        {
            SearchCustommers.Clear();
            for (int i = 0; i < Custommers.Count; i++)
            {
                SearchCustommers.Add(Custommers[i]);
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
            UpdateTotal();
        }

        private void AddCancelClicked(object sender, EventArgs e)
        {
            if (AddProductView.IsVisible == true)
            {
                AddProductView.IsVisible = false;
                UpdateSearchData();
            }


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


                    string data = JsonConvert.SerializeObject(products);
                    App.Current.Properties["Products"] = data;
                    try
                    {
                        string FilePath = Path.Combine(FileSystem.AppDataDirectory, "ProductsData.json");
                        File.WriteAllText(FilePath, data);
                    }
                    catch (Exception h)
                    {

                    }

                }
                else
                {
                    DisplayAlert("Error", "Please Enter the Name, Price, Quantity and make sure you have added atlleast one product", "ok");
                }
            }
            catch (Exception m)
            {
                DisplayAlert("Error", "Input is not in correct format", "ok");
            }
       
            CancelButton.IsEnabled = true;
            CancelButton.Opacity = 1;
            AddProductView.IsVisible = false;
            NewProductNameEntry.Text = "";
            NewProductQuantityEntry.Text = "";
            NewProductPriceEntry.Text = "";
            UpdateSearchData();
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
                    UpdateTotal();
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
                Sum = 0;
                for (int i = 0; i < BillItems.Count; i++)
                {
                    Sum = Sum + BillItems[i].Price * BillItems[i].Quantity;
                }

                try
                {
                    Total = Sum - Convert.ToDouble(DiscountEntry.Text);
                }
                catch (Exception j)
                {
                    Total = Sum;
                }

                SumLabel.Text = Sum.ToString();
                TotalLabel.Text = Total.ToString();


            }
            catch (Exception g)
            {

            }
        }
        private void MainCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainCollectionView.SelectedItem != null)
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
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Name.Contains(e.NewTextValue))
                {
                    SearchItems.Add(products[i]);
                }
            }
        }

        private void DiscountEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotal();
        }

        private void CreateBillButton_Clicked(object sender, EventArgs e)
        {
            if (CustommerNameEntry.Text != "" && CustommerPhoneEntry.Text != "" && BillItems.Count > 0)
            {
                if (SelectedCustommer == null)
                {
                    Custommer custommer = new Custommer()
                    {
                        Name = CustommerNameEntry.Text,
                        PhoneNo = CustommerPhoneEntry.Text
                    };
                    SelectedCustommer = custommer;
                    bool cont = false;

                    if (SearchCustommers.Count == 1)
                    {
                        if (SearchCustommers[0].Name == custommer.Name && SearchCustommers[0].PhoneNo == custommer.PhoneNo)
                        {
                            cont = true;
                        }
                    }

                    if (!cont)
                    {
                        Custommers.Add(custommer);
                        string cdata = JsonConvert.SerializeObject(Custommers);
                        App.Current.Properties["Custommers"] = cdata;
                        try
                        {
                            string FilePath = Path.Combine(FileSystem.AppDataDirectory, "Custommers.json");
                            File.WriteAllText(FilePath, cdata);
                        }
                        catch (Exception n)
                        {

                        }
                    }
                }
                SingleSale sale = new SingleSale()
                {
                    Customer = SelectedCustommer,
                    ItesmsSold = BillItems,
                    date = DateTime.Now,
                    Discount = DiscountEntry.Text,
                    TotalAmount = Total
                };
                SaleData.Add(sale);
                string salesdata = JsonConvert.SerializeObject(SaleData);
                App.Current.Properties["Sales"] = salesdata;
                try
                {
                    string FilePath2 = Path.Combine(FileSystem.AppDataDirectory, "Sales.json");
                    File.WriteAllText(FilePath2, salesdata);
                }
                catch (Exception t)
                {

                }
                string data = JsonConvert.SerializeObject(products);
                App.Current.Properties["Products"] = data;
                try
                {
                    string FilePath = Path.Combine(FileSystem.AppDataDirectory, "ProductsData.json");
                    File.WriteAllText(FilePath, data);
                }
                catch (Exception h)
                {

                }
                SelectedCustommer = null;

                UpdateCustommerData();
                Reset();
            }
            else
            {
                DisplayAlert("Enter Custommer Data", "Please enter custommer name and phone number", "Ok");
            }
        }


        public void Reset()
        {
            SelectedCustommer = null;
            CustommerNameEntry.Text = "";
            CustommerPhoneEntry.Text = "";
            BillItems.Clear();
            DiscountEntry.Text = "0";
        }

        private void CustommerNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchCustommers.Clear();
            //SelectedCustommer = null;
            for (int i = 0; i < Custommers.Count; i++)
            {
                if (Custommers[i].Name.Contains(e.NewTextValue))
                {
                    SearchCustommers.Add(Custommers[i]);
                }
            }
        }

        private void CustommerPhoneEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchCustommers.Clear();
            for (int i = 0; i < Custommers.Count; i++)
            {
                if (Custommers[i].PhoneNo.Contains(e.NewTextValue))
                {
                    SearchCustommers.Add(Custommers[i]);
                }
            }
        }

        private void CustommerCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustommersCollectionView.SelectedItem != null)
            {
                SelectedCustommer = e.CurrentSelection[0] as Custommer;
                CustommerNameEntry.Text = SelectedCustommer.Name;
                CustommerPhoneEntry.Text = SelectedCustommer.PhoneNo;
                

            }
            CustommersCollectionView.SelectedItem = null;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Product product = button.BindingContext as Product;
            AddProductView.IsVisible = true;
            NewProductNameEntry.Text = product.Name;
            NewProductQuantityEntry.Text = product.Quantity.ToString();
            NewProductPriceEntry.Text = product.Price.ToString();
            CancelButton.IsEnabled = false;
            CancelButton.Opacity = 0;
            products.Remove(product);
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Product product = button.BindingContext as Product;
            if (await DisplayAlert("Alert", "Are you sure to delete this item ?", "Yes", "No"))
            {
                products.Remove(product);
                string data = JsonConvert.SerializeObject(products);
                App.Current.Properties["Products"] = data;
                try
                {
                    string FilePath = Path.Combine(FileSystem.AppDataDirectory, "ProductsData.json");
                    File.WriteAllText(FilePath, data);
                }
                catch (Exception h)
                {

                }
                UpdateSearchData();
            }
        }

        private void UpdateViewButton_Clicked(object sender, EventArgs e)
        {
            UpdateView.IsVisible = true;
            UpdateCollectionView.ItemsSource = null;
            UpdateCollectionView.ItemsSource = products;

        }

        private void GoBack_Clicked(object sender, EventArgs e)
        {
            UpdateView.IsVisible = false;
        }
    }
}
