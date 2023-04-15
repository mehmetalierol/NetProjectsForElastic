using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SmsNotificationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmsController : ControllerBase
    {
        private ILogger<SmsController> Logger { get; set; }
        public SmsController(ILogger<SmsController> logger)
        {
            Logger = logger;
        }

        [HttpPost("SendSms")]
        public async Task<IActionResult> SendSms([FromBody] UserModel message)
        {
            Logger.LogWarning(JsonConvert.SerializeObject(message));
            return Ok();
        }
    }
}