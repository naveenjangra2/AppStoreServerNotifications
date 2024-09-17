using Newtonsoft.Json;

namespace appstore.notification.api.Models
{
    public class RenewalInfoV2
    {
        [JsonProperty("autoRenewProductId")]
        public string AutoRenewProductId { get; set; }

        [JsonProperty("autoRenewStatus")]
        public AutoRenewStatus AutoRenewStatus { get; set; }

        [JsonProperty("expirationIntent")]
        public ExpirationIntent ExpirationIntent { get; set; }

        [JsonProperty("gracePeriodExpiresDate")]
        public long GracePeriodExpiresDate { get; set; }

        [JsonProperty("isInBillingRetryPeriod")]
        public bool IsInBillingRetryPeriod { get; set; }

        [JsonProperty("offerIdentifier")]
        public string OfferIdentifier { get; set; }

        [JsonProperty("offerType")]
        public OfferType OfferType { get; set; }

        [JsonProperty("originalTransactionId")]
        public string OriginalTransactionId { get; set; }

        [JsonProperty("priceIncreaseStatus")]
        public PriceConsent PriceIncreaseStatus { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("renewalPrice")]
        public int RenewalPrice { get; set; }

        [JsonProperty("signedDate")]
        public long SignatureDate { get; set; }
    }
}
//{ "originalTransactionId":"2000000681256616","autoRenewProductId":"monthly_basic_ios",
  //      "productId":"kableone_year","autoRenewStatus":1,"renewalPrice":249000,
    //    "currency":"INR","signedDate":1723200233325,
      //  "environment":"Sandbox","recentSubscriptionStartDate":1723196861000,"renewalDate":1723200461000}