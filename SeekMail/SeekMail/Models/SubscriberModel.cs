using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SeekMail.Domain;

namespace SeekMail.Models
{
    public class SubscriberModel
    {
        public SubscriberModel()
        {
        }

        public SubscriberModel(Subscriber entity)
        {
            EmailAddress = entity.EmailAddress;
            Created = entity.Created;
        }

        [Required]
        [EmailAddress]
        [DisplayName("Email address")]
        public string EmailAddress { get; set; }

        [ReadOnly(true)]
        public DateTime Created { get; set; }
    }
}