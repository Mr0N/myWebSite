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

        }
    }
}
