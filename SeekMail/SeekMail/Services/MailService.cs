using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using SeekMail.Domain;

namespace SeekMail.Services
{
    public interface IMailService
    {
        Message Send(IMessageTemplate template, string emailAddress);
    }

    /// <summary>
    /// Note - sending mail is expensive (not too mention there could be lots of emails to send)!
    /// This should really be done using a message queue (NServiceBus service for example).
    /// It would also be better to raise an event when a message is sent, rather than returning a message log object.
    /// </summary>
    public class MailService : IMailService
    {
        readonly ISmtpClient _smtpClient;

        public MailService(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public Message Send(IMessageTemplate template, string emailAddress)
        {
            var mailMessage = createMailMessage(template, emailAddress);
            _smtpClient.Send(mailMessage);
            return new Message(DateTime.Now, emailAddress);
        }

        MailMessage createMailMessage(IMessageTemplate template, string emailAddress)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(template.FromEmail, template.FromName)
            };
            mailMessage.To.Add(new MailAddress(emailAddress));
            mailMessage.Subject = template.Subject;

            // Ensure HTML view is added last as the last view is shown by mail clients
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(template.TextBody, Encoding.UTF8, MediaTypeNames.Text.Plain));
            if (!string.IsNullOrWhiteSpace(template.HtmlBody))
                mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(template.HtmlBody, Encoding.UTF8, MediaTypeNames.Text.Html));

            return mailMessage;
        }
    }
}