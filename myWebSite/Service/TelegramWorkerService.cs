using myWebSite.Models;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace myWebSite.Service
{
    public class TelegramWorkerService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            _client.StartReceiving(HandleUpdateLoadAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                stoppingToken);
            await _client.GetMeAsync(stoppingToken);
        }
        TelegramBotClient _client;
        async Task HandleUpdateLoadAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                await HandleUpdateAsync(botClient, update, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message

            if (update.Message == null) return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            if (messageText == null)
            {
                var sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "You said:\n" + messageText,
                    cancellationToken: cancellationToken);
                return;
            }
            if (messageText.StartsWith("/pass"))
            {
                string password = Regex.Replace(messageText, "^/pass ", "");
                foreach (var item in _pointDb.passwords.Select(a => a.Password))
                {
                    if (password == item)
                    {

                        await _pointDb.users.AddAsync(new Models.TelegramUser()
                        {
                            ChatId = chatId
                        });

                        var sentMessage = await botClient.SendTextMessageAsync(
                                                                             chatId: chatId,
                                                                             text: "Ви  зареєстровані",
                                                                             cancellationToken: cancellationToken);
                        return;
                    }

                }
                await botClient.SendTextMessageAsync(
                                                                chatId: chatId,
                                                                text: "Ваш пароль не найдено",
                                                                cancellationToken: cancellationToken);
                return;
            }
            await botClient.SendTextMessageAsync(
                                                               chatId: chatId,
                                                               text: "Команда не найдена",
                                                               cancellationToken: cancellationToken);
            await _pointDb.SaveChangesAsync();
        }
        PointDbContext _pointDb;

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        public TelegramWorkerService(TelegramBotClient client, PointDbContext pointDb)
        {
            _client = client;
            _pointDb = pointDb;
        }
    }
}
