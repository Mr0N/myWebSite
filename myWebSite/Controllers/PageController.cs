using Microsoft.AspNetCore.Mvc;

namespace myWebSite.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
