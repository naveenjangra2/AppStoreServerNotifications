using System;
using Newtonsoft.Json;
namespace appstore.notification.api.Models
{
	public class SignedTransactionInfo
	{
		[JsonProperty("signedTransactionInfo")]
		public string? SignedTransaction { get; set; }

		public int? errorCode { get; set; }
        public string? errorMessage { get; set; }
    }
}

