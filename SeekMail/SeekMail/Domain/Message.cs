using System;

namespace SeekMail.Domain
{
    public class Message
    {
        protected Message()
        {
        }

        public Message(DateTime sent, string to)
        {
            Sent = sent;
            To = to;
        }

        public int Id { get; set; }
        public DateTime Sent { get; set; }
        public string To { get; set; }
    }
}