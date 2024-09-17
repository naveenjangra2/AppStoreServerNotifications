using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace appstore.notification.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var enctext = EncryptAESString("TEstx", "Fr!d00M0pHa$e2er", "Fr!d00M0pHa$e2er");
            var outtext = DecryptAESString(enctext, "Fr!d00M0pHa$e2er", "Fr!d00M0pHa$e2er");
            Console.WriteLine(outtext);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
           
        }
        public static string EncryptAESString(string plainText, string key, string iv)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes;

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.Close(); encryptedBytes = ms.ToArray();
                }
                return Convert.ToBase64String(encryptedBytes);
            }
        }
        public static string DecryptAESString(string cipherText, string key, string iv)
        {
            try
            {
                using (AesManaged aesAlg = new AesManaged())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    byte[] decryptedBytes;

                    using (var ms = new System.IO.MemoryStream())
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close(); decryptedBytes = ms.ToArray();
                    }
                    var result = Encoding.UTF8.GetString(decryptedBytes);
                    return result;
                }
            }
            catch (Exception ex) { throw; }
        }
    }

}