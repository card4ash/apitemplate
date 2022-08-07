using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index([FromQuery]int userId)
        {
            ViewBag.UserId = userId;
            return View();
        }
    }
}
