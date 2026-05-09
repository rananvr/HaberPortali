using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.MVC.Controllers
{
    public class WebController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            ViewBag.NewsId = id; 
            return View();
    
        }

        public IActionResult Category(int id)
        {
            ViewBag.CategoryId = id;
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

    }

}