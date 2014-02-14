using System.IO;
using System.Linq;
using System.Net.Mail;
using Machine.Fakes;
using Machine.Specifications;
using SeekMail.Domain;
using SeekMail.Services;

namespace SeekMail.Specs.Domain
{
    [Subject(typeof(MailService))]
    public class When_sending_an_email_to_a_subscriber : WithSubject<MailService>
    {
        It should_send_a_mail_message = () =>
             SentMessage.ShouldNotBeNull();

        It should_populate_from_address_from_template = () =>
        {
            SentMessage.From.Address.ShouldEqual("seek@seek.com");
            SentMessage.From.DisplayName.ShouldEqual("Seek");
        };

        It should_populate_to_address_from_subscriber = () =>
            SentMessage.To.ShouldContain(x => x.Address == "joe@bloggs.com");

        It should_populate_subject_from_template = () =>
            SentMessage.Subject.ShouldEqual("Welcome to Seek");

        It should_add_text_view_first = () =>
        {
            var view = SentMessage.AlternateViews.FirstOrDefault();
            view.ShouldNotBeNull();
            view.ContentType.MediaType.ShouldEqual("text/plain");
            view.ContentType.CharSet.ShouldEqual("utf-8");
            getContentString(view.ContentStream).ShouldEqual("text");
        };

        It should_add_html_view_last = () =>
        {
            var view = SentMessage.AlternateViews.LastOrDefault();
            view.ShouldNotBeNull();
            view.ContentType.MediaType.ShouldEqual("text/html");
            view.ContentType.CharSet.ShouldEqual("utf-8");
            getContentString(view.ContentStream).ShouldEqual("<html>");
        };

        static string getContentString(Stream stream)
        {
            var buffer = new byte[stream.Length];
            return System.Text.Encoding.Default.GetString(buffer, 0, stream.Read(buffer, 0, buffer.Length));
        }

        Establish context = () =>
        {
            The<ISmtpClient>().WhenToldTo(x => x.Send(Param<MailMessage>.IsAnything))
                .Callback<MailMessage>(x => SentMessage = x);
        };

        Because of = () =>
            Subject.Send(
                new MessageTemplate
                {
                    FromEmail = "seek@seek.com",
                    FromName = "Seek",
                    Subject = "Welcome to Seek",
                    TextBody = "text",
                    HtmlBody = "<html>"
                },
                "joe@bloggs.com");

        static MailMessage SentMessage;
    }
}