using Newtonsoft.Json;

namespace appstore.notification.api.Models
{
    public class TransactionInfoV2
    {
        [JsonProperty("appAccountToken")]
        public string AppAccountToken { get; set; }

        [JsonProperty("bundleId")]
        public string BundleId { get; set; }

        [JsonProperty("expiresDate")]
        public long ExpiresDate { get; set; }

        [JsonProperty("inAppOwnershipType")]
        public OwnershipType InAppOwnershipType { get; set; }

        [JsonProperty("isUpgraded")]
        public bool IsUpgraded { get; set; }

        [JsonProperty("offerIdentifier")]
        public string OfferIdentifier { get; set; }

        [JsonProperty("offerType")]
        public OfferType OfferType { get; set; }

        [JsonProperty("originalPurchaseDate")]
        public long OriginalPurchaseDate { get; set; }

        [JsonProperty("originalTransactionId")]
        public long OriginalTransactionId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("purchaseDate")]
        public long PurchaseDate { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("revocationDate")]
        public long RevocationDate { get; set; }

        [JsonProperty("revocationReason")]
        public CancellationReason RevocationReason { get; set; }

        [JsonProperty("subscriptionGroupIdentifier")]
        public string SubscriptionGroupIdentifier { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("type")]
        public PurchaseType Type { get; set; }

        [JsonProperty("webOrderLineItemId")]
        public string WebOrderLineItemId { get; set; }

        [JsonProperty("signedDate")]
        public long SignatureDate { get; set; }
    }
}
/*{ "transactionId":"2000000681256616","originalTransactionId":"2000000681256616",
        "webOrderLineItemId":"2000000070326834","bundleId":"com.kableone.tveverywhere",
        "productId":"kableone_year","subscriptionGroupIdentifier":"20793586",
        "purchaseDate":1723196861000,"originalPurchaseDate":1723196862000,
        "expiresDate":1723200461000,"quantity":1,"type":"Auto-Renewable Subscription",
        "inAppOwnershipType":"PURCHASED","signedDate":1723200233325,"environment":
        "Sandbox","transactionReason":"PURCHASE","storefront":"IND","storefrontId":"143467","price":4000000,"currency":"INR"}*/