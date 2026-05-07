using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.UI.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
