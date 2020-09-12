using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoreApp.Billing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BillPane : ContentView
    {
        public BillPane()
        {
            InitializeComponent();
        }
    }
}