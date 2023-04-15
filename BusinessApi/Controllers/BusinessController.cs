using Common;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net.Http.Headers;
using System.Text;

namespace BusinessApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private ILogger<BusinessController> Logger;
        public BusinessController(ILogger<BusinessController> logger)
        {
            Logger = logger;
        }

        [HttpPost("StartProcess")]
        public async Task<IActionResult> SendMessage()
        {
            await RedisFunc();
            var response = await EntityApiCall();
            Logger.LogWarning(JsonConvert.SerializeObject(response));

            var json = JsonConvert.DeserializeObject<UserModel>(response);

            if (json != null)
                await Send(json);

            return Ok();
        }

        private async Task<string> EntityApiCall()
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:5003/Crud/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetStringAsync("GetUser");
        }

        private async Task Send(UserModel user)
        {
            Logger.LogWarning("Azure Service Bus Producer called.");
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:5002/ServiceBusOps/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("SendMessage", data);
        }

        private async Task RedisFunc()
        {
            Logger.LogWarning("Redis function called.");
            var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions { EndPoints = { "localhost:6379" } });
            redis.UseElasticApm();
            var db = redis.GetDatabase();
            var asd = await db.PingAsync();
        }
    }
}