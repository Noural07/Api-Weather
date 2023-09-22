using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Weather.Models;
using System.Net.Http;
using static Weather.Models.WeatherD;

namespace Weather.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                using HttpClient client = new HttpClient();

                // Ensure that you use "https://" in the request URI
                string requestUri = "https://api.openweathermap.org/data/2.5/weather?q=London,uk&APPID=845ffe74df89f2cec797b94d329c7e86";

                HttpResponseMessage response = await client.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Root rootObject = JsonSerializer.Deserialize<Root>(jsonResponse);
                    return View(rootObject);
                }
                else
                {
                    // Handle the case where the HTTP request was not successful
                    ViewData["ErrorMessage"] = "Unable to fetch weather data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the request
                ViewData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return View("Error");
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}