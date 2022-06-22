using Microsoft.EntityFrameworkCore;

namespace myWebSite.Models
{
    public class PointDbContext:DbContext
    {
        public DbSet<TelegramUser> users { get; set; }
        public DbSet<PasswordTelegramBot> passwords { set; get; }
        public DbSet<MessageFromClient> messages { set; get; }
        public PointDbContext(DbContextOptions options):base(options)
        {
            Console.WriteLine("DB Create");
            Database.EnsureCreated();
            if (passwords.Count() == 0)
            {
                passwords.Add(new PasswordTelegramBot() { Password = "user!@1998" });
                SaveChanges();
            }
            
        }
    }
}
