using Apple.Receipt.Parser.Services;
using appstore.notification.api.Interfaces;
using appstore.notification.api.Models;
using Microsoft.AspNetCore.Mvc;
namespace appstore.notification.api.Controllers
{
    [Route("notification")]
    public class AppStoreNotificationController : Controller
    {
        private readonly INotificationProcessor _notificationProcessor;
        private readonly IAppleReceiptParserService _parserService;

        public AppStoreNotificationController(INotificationProcessor notificationProcessor, IAppleReceiptParserService parserService)
        {
            _notificationProcessor = notificationProcessor;
            _parserService = parserService;

        }

        [HttpPost("appstore")]
        public ActionResult Notification([FromBody]  AppleNotification appleNotification)
        {
            try
            {
                _notificationProcessor.Process(appleNotification);
                
                return Ok();
            }
            catch(Exception e)
            {
                return  StatusCode(500);
            }
        }
        [HttpPost("read")]
        public ActionResult ReadReciept([FromBody] AppleNotification appleNotification)
        {
            try
            {
                _notificationProcessor.ReadReciept(appleNotification, _parserService);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
