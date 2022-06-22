using myWebSite.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace myWebSite.Service
{
    public class MessageToTelegramService
    {
        public async Task Start(string name,string telegram)
        {
            foreach (var item in _pointDb.users
                //.SelectMany(a=>a.users)
                .Select(a=>a.ChatId))
            {
                await _client.SendTextMessageAsync(new ChatId(item),
                    $"Name:{name}\n\r" +
                    $"Telegram:{telegram}");
            }
            
        }
        PointDbContext _pointDb;
        TelegramBotClient _client;
        public MessageToTelegramService(PointDbContext pointDb, TelegramBotClient client)
        {
            this._pointDb = pointDb;
            this._client = client;
        }
    }
}
