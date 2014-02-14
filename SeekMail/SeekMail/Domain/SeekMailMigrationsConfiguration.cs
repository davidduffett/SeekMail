using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SeekMail.Domain
{
    public class SeekMailMigrationsConfiguration : DbMigrationsConfiguration<SeekMailContext>
    {
        public SeekMailMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true; // NOTE: Don't go live with this!
        }

        /// <summary>
        /// Seed with some sample data.
        /// </summary>
        protected override void Seed(SeekMailContext context)
        {
            base.Seed(context);

            if (!context.MessageTemplates.Any())
            {
                context.MessageTemplates.Add(new MessageTemplate
                {
                    Created = DateTime.Now,
                    Name = "Our first template",
                    FromName = "Seek",
                    FromEmail = "emails@seek.com",
                    Subject = "Welcome to Seek!",
                    TextBody = "This template only has a text template."
                });
            }

            if (!context.Subscribers.Any())
            {
                context.Subscribers.Add(new Subscriber { EmailAddress = "joe@bloggs.com" });
            }

            // Add sent messages to demonstrate paging
            if (!context.Messages.Any())
                context.Messages.AddRange(Enumerable.Range(1, 100)
                    .Select(i => new Message(DateTime.Now, string.Format("subscriber{0}@gmail.com", i))));

            context.SaveChanges();
        }
    }
}