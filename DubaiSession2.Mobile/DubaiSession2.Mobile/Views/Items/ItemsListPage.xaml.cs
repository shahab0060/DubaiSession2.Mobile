using DubaiSession2.Mobile.Methods;
using DubaiSession2.Mobile.ViewModels.Item;
using DubaiSession2.Mobile.Views.ItemPrice;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DubaiSession2.Mobile.Views.Items
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsListPage : ContentPage
    {
        private readonly HttpClient _client;
        public ItemsListPage()
        {
            _client = BaseInformation.GetHttpClient();
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            string json = await _client.GetStringAsync($"Item/index/{BaseInformation.CurrentUserId}");
            List<ShowItemBaseInformationViewModel> items = JsonConvert.DeserializeObject<List<ShowItemBaseInformationViewModel>>(json);
            ItemsListView.ItemsSource = items;
            base.OnAppearing();
        }

        private async void ChangePriceButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var bindingContext = (ShowItemBaseInformationViewModel)button.BindingContext;
            await Navigation.PushModalAsync(new UpsertItemPricePage(bindingContext.Id,bindingContext.Title));
        }
    }
}