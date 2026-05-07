using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.UI.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
