using System;
using System.ComponentModel.DataAnnotations;

namespace SeekMail.Domain
{
    public class Subscriber
    {
        public Subscriber()
        {
            Created = DateTime.Now;
        }

        [Key]
        public string EmailAddress { get; set; }
        public DateTime Created { get; set; }
    }
}