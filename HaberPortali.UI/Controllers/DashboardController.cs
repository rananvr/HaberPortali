using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.UI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
