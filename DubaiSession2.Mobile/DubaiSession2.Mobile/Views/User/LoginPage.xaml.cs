using DubaiSession2.Mobile.Methods;
using DubaiSession2.Mobile.ViewModels.User;
using DubaiSession2.Mobile.Views.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DubaiSession2.Mobile.Views.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        #region constructor

        private readonly HttpClient _client;

        #endregion
        public LoginPage()
        {
            _client = BaseInformation.GetHttpClient();
            InitializeComponent();
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            LoginUserViewModel login = new LoginUserViewModel()
            {
                Password = PasswordInput.Text,
                UserName = UserNameInput.Text,
            };
            string json = JsonConvert.SerializeObject(login);
            var stringContent = new StringContent(json,Encoding.UTF8,"application/json");
            var result = await _client.PostAsync("User/login",stringContent);
            if(result.IsSuccessStatusCode)
            {
                string res = await result.Content.ReadAsStringAsync();
                long userId = JsonConvert.DeserializeObject<long>(res);
                await DisplayAlert("success", "login success", "ok");
                BaseInformation.CurrentUserId = userId;
                await Navigation.PushModalAsync(new ItemsListPage());
                return;
            }
            await DisplayAlert("error", await result.Content.ReadAsStringAsync(), "ok");
        }
    }
}