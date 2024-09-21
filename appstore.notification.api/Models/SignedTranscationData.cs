using System;
using Newtonsoft.Json;

namespace appstore.notification.api.Models
{
	public class SignedTranscationData
	{
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }

        [JsonProperty("webOrderLineItemId")]
        public string WebOrderLineItemId { get; set; }

        [JsonProperty("bundleId")]
        public string BundleId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("subscriptionGroupIdentifier")]
        public string SubscriptionGroupIdentifier { get; set; }

        [JsonProperty("PurchaseDate")]
        public long PurchaseDate { get; set; }

        [JsonProperty("originalPurchaseDate")]
        public long OriginalPurchaseDate { get; set; }

        [JsonProperty("expiresDate")]
        public long ExpiresDate { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("inAppOwnershipType")]
        public string InAppOwnershipType { get; set; }

        [JsonProperty("signedDate")]
        public long SignedDate { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("transactionReason")]
        public string TransactionReason { get; set; }

        [JsonProperty("storefront")]
        public string Storefront { get; set; }

        [JsonProperty("storefrontId")]
        public string StorefrontId { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}

/*
{ "transactionId":"70001552423108","originalTransactionId":"70001552423108",
		"webOrderLineItemId":"70000703012947",
		"bundleId":"com.kableone.tveverywhere",
		"productId":"saga_plus_ios_1","subscriptionGroupIdentifier":"20793586",
		"purchaseDate":1673634973000,"originalPurchaseDate":1673634975000,
		"expiresDate":1676313373000,"quantity":1,"type":"Auto-Renewable Subscription",
		"inAppOwnershipType":"PURCHASED","signedDate":1726897081714,"environment":"Production",
		"transactionReason":"PURCHASE","storefront":"IND",
		"storefrontId":"143467","price":499000,"currency":"INR"}*/