using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
