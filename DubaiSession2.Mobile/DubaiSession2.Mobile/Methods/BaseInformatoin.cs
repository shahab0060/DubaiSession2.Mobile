using System;
using System.Net.Http;

namespace DubaiSession2.Mobile.Methods
{
    public static class BaseInformation
    {
        public static long CurrentUserId;
        public static HttpClient GetHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.1.168:5039/");
            return httpClient;
        }
    }
}
