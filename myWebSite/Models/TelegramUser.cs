namespace myWebSite.Models
{
    public class TelegramUser
    {
        public int Id { get; set; }

        public ICollection<User> users { get; set; }
    }
}
