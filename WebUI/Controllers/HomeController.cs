using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartProcess()
        {
            await Send();

            return View("Index");
        }

        private async Task Send()
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:5004/Business/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsync("StartProcess", null);
        }
    }
}