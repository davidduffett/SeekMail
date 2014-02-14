using System;

namespace SeekMail.Domain
{
    public interface IMessageTemplate
    {
        string FromEmail { get; set; }
        string FromName { get; set; }
        string Subject { get; set; }
        string HtmlBody { get; set; }
        string TextBody { get; set; }
    }

    public class MessageTemplate : IMessageTemplate
    {
        public MessageTemplate()
        {
            Created = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
    }
}