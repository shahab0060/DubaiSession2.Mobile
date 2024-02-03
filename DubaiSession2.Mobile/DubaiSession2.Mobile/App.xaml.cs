using DubaiSession2.Mobile.Views.ItemPrice;
using DubaiSession2.Mobile.Views.User;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DubaiSession2.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
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
