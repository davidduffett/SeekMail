using System.Data.Entity;

namespace SeekMail.Domain
{
    public class SeekMailContext : DbContext
    {
        public SeekMailContext() : base("SeekMailDatabase")
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SeekMailContext, SeekMailMigrationsConfiguration>());
        }

        public DbSet<MessageTemplate> MessageTemplates { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}