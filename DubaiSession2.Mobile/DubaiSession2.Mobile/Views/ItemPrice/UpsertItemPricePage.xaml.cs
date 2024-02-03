using DubaiSession2.Mobile.Methods;
using DubaiSession2.Mobile.ViewModels;
using DubaiSession2.Mobile.ViewModels.Item;
using DubaiSession2.Mobile.Views.Items;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DubaiSession2.Mobile.Views.ItemPrice
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpsertItemPricePage : ContentPage
    {
        private readonly HttpClient _client;
        private readonly long _itemId;
        private readonly string _itemTitle;
        bool isEditing = false;
        public UpsertItemPricePage(long itemId,string itemTitle)
        {
            _itemTitle = itemTitle;
            _itemId = itemId;
            _client = BaseInformation.GetHttpClient();
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            ItemTitleLbl.Text = _itemTitle;
            ItemPricesListView.IsRefreshing = true;
            string json = await _client.GetStringAsync($"ItemPrice/index/{_itemId}");
            List<ShowItemPriceBaseInformationViewModel> itemPrices = JsonConvert.DeserializeObject<List<ShowItemPriceBaseInformationViewModel>>(json);
            ItemPricesListView.ItemsSource = itemPrices;

            string cancelsJson = await _client.GetStringAsync("Cancell");
            List<CancelPolictyInformationViewModel>cancels = JsonConvert.DeserializeObject<List<CancelPolictyInformationViewModel>>(cancelsJson);
            BindingContext = cancels;
            ItemPricesListView.IsRefreshing = false;
            base.OnAppearing();
        }

        private async void SetPriceBtn_Clicked(object sender, System.EventArgs e)
        {
            if(isEditing)
            {
                Edit();
                return;
            }
            if(HolidaysPolicy.SelectedItem is null ||
                NormalDaysPolicy.SelectedItem is null ||
                WeekendPolicy.SelectedItem is null)
            {
                await DisplayAlert("error", "please select all the policies", "ok");
                return;
            }
            CreateItemPriceInformation create = new CreateItemPriceInformation()
            {
                FromDate = FromDateInput.Date,
                ToDate = ToDateInput.Date,
                HolidayCancellationPolicyId = ((CancelPolictyInformationViewModel)HolidaysPolicy.SelectedItem).Id,
                HolidayPrice = (decimal)HolidaysPriceInput.Value,
                WeekendCancellationPolicyId = ((CancelPolictyInformationViewModel)WeekendPolicy.SelectedItem).Id,
                WeekendPrice = (decimal)WeekendPriceInput.Value,
                NormalDayCancellationPolicyId = ((CancelPolictyInformationViewModel)NormalDaysPolicy.SelectedItem).Id,
                NormalDayPrice = (decimal)NormalDaysPriceInput.Value,
                ItemId = _itemId
            };
            string json = JsonConvert.SerializeObject(create);
            var stringContent = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var result = await _client.PostAsync("/ItemPrice/create/itemPrice", stringContent);
            if(result.IsSuccessStatusCode)
            {
                await DisplayAlert("success", "your information saved successfully", "ok");
                await Navigation.PushModalAsync(new UpsertItemPricePage(_itemId,_itemTitle));
                return;
            }
            await DisplayAlert("error", await result.Content.ReadAsStringAsync(), "ok");

        }

        private async void Edit()
        {
            await DisplayAlert("oopos", "this function have not been coded for lack of time", "it's ok shahab do not overthink about it");
            return;
        }

        private async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new ItemsListPage());
        }

        private async void RemoveBtn_Clicked(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var bindingContext = (ShowItemPriceBaseInformationViewModel)button.BindingContext;
            long id = bindingContext.Id;
            string json = JsonConvert.SerializeObject(id);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("ItemPrice", stringContent);
            if(result.IsSuccessStatusCode)
            {
                await DisplayAlert("success", "removed successfully", "ok");
                await Navigation.PushModalAsync(new UpsertItemPricePage(_itemId,_itemTitle));
                return;
            }
            await DisplayAlert("error", await result.Content.ReadAsStringAsync(), "ok");
        }

        private async void UpdateBtn_Clicked(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var bindingContext = (ShowItemPriceBaseInformationViewModel)button.BindingContext;
            long id = bindingContext.Id;
            string json =await _client.GetStringAsync($"ItemPrice/edit/information/{id}");
            EditItemPriceInformation information = JsonConvert.DeserializeObject<EditItemPriceInformation>(json);
            FromDateInput.Date = information.FromDate;
            ToDateInput.Date = information.ToDate;
            HolidaysPriceInput.Value = (double)information.HolidayPrice;
            WeekendPriceInput.Value = (double)information.WeekendPrice;
            NormalDaysPriceInput.Value = (double)information.NormalDayPrice;
            HolidaysPolicy.SelectedItem = ((List<CancelPolictyInformationViewModel>)BindingContext)
                                            .Where(a=>a.Id==information.HolidayCancellationPolicyId).FirstOrDefault();
            WeekendPolicy.SelectedItem = ((List<CancelPolictyInformationViewModel>)BindingContext)
                                            .Where(a => a.Id == information.WeekendCancellationPolicyId).FirstOrDefault();
            NormalDaysPolicy.SelectedItem = ((List<CancelPolictyInformationViewModel>)BindingContext)
                                            .Where(a => a.Id == information.NormalDayCancellationPolicyId).FirstOrDefault();
            isEditing = true;
        }
    }
}