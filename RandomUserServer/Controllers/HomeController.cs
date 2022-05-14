using Microsoft.AspNetCore.Mvc;

namespace RandomUserServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
