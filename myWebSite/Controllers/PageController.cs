using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;

namespace myWebSite.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Get(string uri)
        {
            var bytes = Convert.FromBase64String(uri);
            uri = Encoding.UTF8.GetString(bytes);
            if(Uri.TryCreate(uri,new UriCreationOptions(),out var result))
            {
                ViewBag.Domen = result.Host;
                return View("Index");
            }
            return BadRequest();
        }
    }
}
