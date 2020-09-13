﻿using StoreApp.Api;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoreApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ApiHelper.InitialiseClient();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
