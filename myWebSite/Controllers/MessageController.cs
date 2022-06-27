using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> Send(MessageFromClient message)
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
                await _message.Start(message.Telegram, message.Name);
            }
            catch(Exception ex) {
                _loger.LogWarning(ex?.Message ?? "");
            }
            return View("Index");
        }
        PointDbContext _pointDb;
        MessageToTelegramService _message;
        ILogger<MessageController> _loger;
        public MessageController(PointDbContext pointDb, MessageToTelegramService message,ILogger<MessageController> logger)
        {
            _pointDb = pointDb;
            _message = message;
            _loger = logger;
        }
    }
}
