using appstore.notification.api.Interfaces;
using appstore.notification.api.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Apple.Receipt.Parser.Modules;
using Apple.Receipt.Models;
using Apple.Receipt.Parser.Services;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;

namespace appstore.notification.api.Services
{
    public class NotificationProcessor : INotificationProcessor
    {
        private readonly ISubsciptionService _subsciptionService;

        public NotificationProcessor(ISubsciptionService subsciptionService)
        {
            _subsciptionService = subsciptionService;
        }

        public void Process(AppleNotification notification)
        {
            var v2Notification = GetVerifiedDecodedData<NotificationV2>(notification.SignedPayload);

            if (v2Notification?.DecodedPayload?.Data == null || !v2Notification.IsValid)
            {
                throw new ArgumentNullException($"{nameof(v2Notification.DecodedPayload.Data)} is null or is not valid");
            }

            RenewalInfoV2? renewalInfo = null;
            if (!string.IsNullOrEmpty(v2Notification.DecodedPayload.Data.SignedRenewalInfo))
            {
                var renewalInfoV2Verified = GetVerifiedDecodedData<RenewalInfoV2>(v2Notification.DecodedPayload.Data.SignedRenewalInfo);
                if(renewalInfoV2Verified.IsValid) renewalInfo = renewalInfoV2Verified.DecodedPayload;
            }

            var transactionInfoResponse = GetVerifiedDecodedData<TransactionInfoV2>(v2Notification.DecodedPayload.Data.SignedTransactionInfo);
            TransactionInfoV2? transactionInfo = null;
            if (transactionInfoResponse.IsValid) transactionInfo = transactionInfoResponse.DecodedPayload;
            
            // Update Internal subscription
            _subsciptionService.Update(v2Notification.DecodedPayload, renewalInfo, transactionInfo);
        }
        public void ReadReciept(AppleNotification notification, IAppleReceiptParserService parserService)
        {
           
            string base64Receipt = notification.SignedPayload;
            byte[] receiptData = Convert.FromBase64String(base64Receipt);

            AppleAppReceipt? receipt = parserService.GetAppleReceiptFromBytes(receiptData); ;
            if (receipt == null)  { return; }
            var d = receipt.PurchaseReceipts.OrderByDescending(var => var.ExpirationDate).ToList();
            var recipetModel = d.FirstOrDefault();
            var recieptType = receipt.ReceiptType;
            try
            {
                SignedTranscationData transactionData = ReadRecieptData(recieptType, recipetModel.TransactionId);
                if (transactionData != null){

                }
            }
            catch (Exception e) {
                //save transaction id for error log
            }
            // ...
        }
        private SignedTranscationData ReadRecieptData(String recieptType,String transactiodId)
        {
           
            String url ;

            if (recieptType == "Production") {
                url = "https://api.storekit.itunes.apple.com/inApps/v1/transactions/" + transactiodId;
            }
            else
            {
                url = "https://api.storekit-sandbox.itunes.apple.com/inApps/v1/transactions/" + transactiodId;
            }
            
            var request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer "+ GenerateInAppleToken());
            var sendresponse = request.GetResponse();
            using (var streamReader = new StreamReader(sendresponse.GetResponseStream()))
            {
                var sendresponsetext = streamReader.ReadToEnd().Trim();
                var signedTransactionInfo = JsonConvert.DeserializeObject<SignedTransactionInfo>(sendresponsetext);
                if (signedTransactionInfo != null && signedTransactionInfo.errorMessage == null)
                {
                    var transactionData = GetVerifiedDecodedData<SignedTranscationData>(signedTransactionInfo.SignedTransaction);

                    if (transactionData?.DecodedPayload == null || !transactionData.IsValid)
                    {
                        throw new ArgumentNullException($"{nameof(transactionData.DecodedPayload)} is null or is not valid");
                    }
                    return transactionData.DecodedPayload;
                     
                }
            }
            return null;
        }
        public string GenerateInAppleToken()
        {


            try
            {
                const string iss = "f313d73d-160c-4b70-b9ad-5cf7c443b69b"; // your account's team ID found in the dev portal
                const string aud = "appstoreconnect-v1";
                const string sub = "com.kableone.tveverywhere"; // same as client_id
                var now = DateTime.UtcNow;

                // contents of your .p8 file
                const string privateKey = "MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgnCqUVtqjuNJ75ht1\n4OLP56L7HqURUF+QnK33czKpHgegCgYIKoZIzj0DAQehRANCAAThBNRHiBXbST4L\nBquWNhXXIYxXpRUOxIY2575GuMLIgt0si8oblYhjCST5t83RaKy8qLA/OEuJDmas\nbVVwUD22";
                var ecdsa = ECDsa.Create();
                ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);

                var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
                var token = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = iss,
                    Audience = aud,

                    Claims = new Dictionary<string, object> { { "bid", sub } },
                    Expires = now.AddMinutes(20), // expiry can be a maximum of 6 months - generate one per request or re-use until expiration
                    IssuedAt = now,
                    NotBefore = now.AddMinutes(-1),
                    SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa) { KeyId = "92W797DCPN" }, SecurityAlgorithms.EcdsaSha256)
                });

                return token;
            }
            catch (Exception EXC)
            {
               
            }
            return null;
        }

        private VerifiedDecodedDataModel<TNotificationData> GetVerifiedDecodedData<TNotificationData>(string signedPayload)
        {
            if (string.IsNullOrEmpty(signedPayload))
            {
                throw new ArgumentNullException("Signed Payload is null");
            }

            var splitParts = signedPayload.Split('.'); // JWS header, payload, and signature representations

            EnsurePartElements(splitParts);

            var valid = VerifyToken(signedPayload);
            
            var payload = splitParts[1];

            return new VerifiedDecodedDataModel<TNotificationData> 
            {
                DecodedPayload = valid ? DecodeFromBase64<TNotificationData>(payload) : default,
                IsValid = valid
            };
        }

        private static void EnsurePartElements(string[] split)
        {
            if (split.Length != 3)
            {
                throw new ArgumentException("Invalid signedPayload");
            }

            if (string.IsNullOrEmpty(split[0]))
            {
                throw new ArgumentException("Invalid jws_header part");
            }

            if (string.IsNullOrEmpty(split[1]))
            {
                throw new ArgumentException("Invalid jws_payload part");
            }

            if (string.IsNullOrEmpty(split[2]))
            {
                throw new ArgumentException("Invalid jws_signature part");
            }
        }

        private static bool VerifyToken(string token)
        {
           // return true;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);


                var x5cTry = jwtSecurityToken.Header.TryGetValue("x5c", out var x5cCerteficates);
                var headerJson = JsonConvert.SerializeObject(jwtSecurityToken.Header);
                var data = JsonConvert.DeserializeObject<JSonData>(headerJson);
                if (!x5cTry || x5cCerteficates == null)
                {
                    throw new KeyNotFoundException("Token Header does not contain x5c");
                }
                var certiticates = x5cCerteficates.ToString().Trim();
                /*    var certeficatesItems = JsonConvert.DeserializeObject<IEnumerable<string>>(certiticates);
                    IEnumerable<string> deserializedlistobj = (IEnumerable<string>)JsonConvert.DeserializeObject(certiticates, typeof(IEnumerable<string>));
                    if (deserializedlistobj == null || !deserializedlistobj.Any())
                    {
                        throw new ArgumentNullException("Certeficates are null");
                    }


                    var securityToken = Validate(handler, token, deserializedlistobj.First());*/
                var securityToken = Validate(handler, token, data.x5c.First());

                return securityToken != null;

            }
            catch (Exception ex)
            {
                // log it
                return false;
            }
        }
        public class JSonData
        {
            public string alg { get; set; }
            public List<string> x5c { get; set; }
        }
        private static SecurityToken? Validate(JwtSecurityTokenHandler tokenHandler, string jwtToken, string publicKey)
        {
            var certificateBytes = Base64UrlEncoder.DecodeBytes(publicKey);
            var certificate = new X509Certificate2(certificateBytes);
            var eCDsa = certificate.GetECDsaPublicKey();
            
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new ECDsaSecurityKey(eCDsa),
            };

            tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var securityToken);
            return securityToken;
        }

        private static TObj DecodeFromBase64<TObj>(string encodedString)
        {
            var data = Base64UrlTextEncoder.Decode(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);

            var obj = JsonConvert.DeserializeObject<TObj>(decodedString);
            return obj;
        }

    }
    class HeaderData
    {

    }
}
