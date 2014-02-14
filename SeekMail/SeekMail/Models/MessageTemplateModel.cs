using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SeekMail.Domain;

namespace SeekMail.Models
{
    public class MessageTemplateModel
    {
        public MessageTemplateModel()
        {
        }

        public MessageTemplateModel(MessageTemplate entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Created = entity.Created;
            FromName = entity.FromName;
            FromEmail = entity.FromEmail;
            Subject = entity.Subject;
            HtmlBody = entity.HtmlBody;
            TextBody = entity.TextBody;
        }

        [ReadOnly(true)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ReadOnly(true)]
        public DateTime Created { get; set; }

        [DisplayName("From name")]
        public string FromName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("From email")]
        public string FromEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [DisplayName("HTML body")]
        public string HtmlBody { get; set; }

        [Required]
        [DisplayName("Text body")]
        public string TextBody { get; set; }
    }
}