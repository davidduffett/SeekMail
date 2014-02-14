using System.Net.Mail;

namespace SeekMail.Services
{
    public interface ISmtpClient
    {
        void Send(MailMessage mailMessage);
    }

    /// <summary>
    /// Sends a mail message using the .NET SMTP client, using settings in the application configuration file.
    /// </summary>
    public class NetSmtpClient : ISmtpClient
    {
        public void Send(MailMessage mailMessage)
        {
            using (var client = new SmtpClient())
                client.Send(mailMessage);
        }
    }
}