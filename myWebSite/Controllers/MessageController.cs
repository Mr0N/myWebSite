using Microsoft.AspNetCore.Mvc;
using myWebSite.Models;
using myWebSite.Service;

namespace myWebSite.Controllers
{
   
    public class MessageController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Send(MessageFromClient message)
        {
            _pointDb.messages.Add(new MessageFromClient()
            {
                Name = message.Name,
                Telegram = message.Telegram,
                DateAdds = DateTime.Now
            });
            _pointDb.SaveChanges();
            try
            {
                _message.Start(message.Telegram, message.Name).GetAwaiter().GetResult();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return View("Index");
        }
        PointDbContext _pointDb;
        MessageToTelegramService _message;
        public MessageController(PointDbContext pointDb, MessageToTelegramService message)
        {
            _pointDb = pointDb;
            _message = message;
        }
    }
}
