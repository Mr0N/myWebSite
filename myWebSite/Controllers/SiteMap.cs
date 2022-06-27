using Microsoft.AspNetCore.Mvc;
using SimpleSiteMap;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace myWebSite.Controllers
{
    public class SiteMapController:Controller
    {
        public IActionResult Index(int page,int count)
        {

            // 1. Grab our *partioned* data.
            //    We have 1 million products - so don't return EVERYTHING,
            //    just the 1st, 25,001, 50,001, 75,001, etc
            var date = DateTime.Now.AddDays(-1);
            var dateUpdate = new DateTime(date.Year, date.Month, date.Day);
            string file = Path.Combine(_environment.WebRootPath, "load.txt");
            var result = System.IO.File.ReadAllLines(file)
                .Skip(page*count).Take(count)
                .Select(a =>
                {
                    try
                    {
                        string info = Convert.ToBase64String(Encoding.UTF8.GetBytes(a));
                        string link = "http://" + base.Request.Host.Host + Url.Action("Get", "Page", new { uri = info });
                        return new SitemapNode(new Uri(link), dateUpdate);
                    }
                    catch(Exception ex)
                    {
                        return null;
                    }
                }).Where(a => a != null);
            // 3. Create the sitemap service.
            var sitemapService = new SitemapService();

            // 4. Get the sitemap answer! BOOM!
            var xml = sitemapService.ConvertToXmlSitemap(result.ToList());

            // 5. Return the result as xml.
            return Content(xml, "application/xml");
        }
        IHostingEnvironment _environment;
        public SiteMapController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
    }
}
