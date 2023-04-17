using Microsoft.AspNetCore.Mvc;

namespace LoginDemo.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
