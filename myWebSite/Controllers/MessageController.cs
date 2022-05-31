using Microsoft.AspNetCore.Mvc;
using myWebSite.Models;
using myWebSite.Service;

namespace myWebSite.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index(string telegram, string name)
        {
            _pointDb.messages.Add(new MessageFromClient()
            {
                Name = name,
                Telegram = telegram,
                DateAdds = DateTime.Now
            });
            _pointDb.SaveChanges();
            _message.Start(telegram, name).GetAwaiter().GetResult();
            return View();
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
