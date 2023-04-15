using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MailNotificationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private ILogger<MailController> Logger { get; set; }

        public MailController(ILogger<MailController> logger)
        {
            Logger = logger;
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody] UserModel userModel)
        {
            Logger.LogWarning(JsonConvert.SerializeObject(userModel));
            await InsertLog(userModel);
            return Ok();
        }

        private async Task InsertLog(UserModel userModel)
        {
            Logger.LogWarning("Insert Email Log to postgres Called.");
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:5003/Crud/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("InsertMailLog", data);
        }
    }
}