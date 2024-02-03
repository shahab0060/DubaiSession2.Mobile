using System;

namespace DubaiSession2.Mobile.ViewModels.Item
{
    public class BaseUpserItemPriceInformation
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public decimal? WeekendPrice { get; set; }

        public decimal? HolidayPrice { get; set; }

        public decimal NormalDayPrice { get; set; }

        public long? WeekendCancellationPolicyId { get; set; }

        public long? HolidayCancellationPolicyId { get; set; }

        public long NormalDayCancellationPolicyId { get; set; }
    }
    public class CreateItemPriceInformation : BaseUpserItemPriceInformation
    {
        public long ItemId { get; set; }

    }

    public class EditItemPriceInformation : BaseUpserItemPriceInformation
    {
        public long ItemPriceId { get; set; }
    }
}
