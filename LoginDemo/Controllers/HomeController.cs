using LoginDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace LoginDemo.Controllers
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
        public async Task<IActionResult> LoginUser(LoginModel Login)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Login), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:5000/api/Authenticate/login", content))
                {
                    string token = await response.Content.ReadAsStringAsync();  
                    if(token == "Invalid")
                    {
                        return Redirect("~/Home/Index");
                    }
                    HttpContext.Session.SetString("JWTtoken", token);
                }
                var accestoken = HttpContext.Session.GetString("JWTtoken");
                return Redirect("~/Dashboard/Index");
                
            }
            
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel regMod)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(regMod), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:5000/api/Authenticate/register", content))
                {
                    return Redirect("~/Home/Index");
                }

            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Home/Index");
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