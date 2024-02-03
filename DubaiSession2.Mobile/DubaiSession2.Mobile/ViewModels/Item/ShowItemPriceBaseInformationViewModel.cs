using System;

namespace DubaiSession2.Mobile.ViewModels.Item
{
    public class ShowItemPriceBaseInformationViewModel
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public string CancellationPolicyTitle { get; set; }

        public string Status { get; set; }
    }
}
