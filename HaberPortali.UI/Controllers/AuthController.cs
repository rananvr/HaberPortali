using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.UI.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
