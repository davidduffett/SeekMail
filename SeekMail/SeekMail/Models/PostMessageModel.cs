using System.ComponentModel.DataAnnotations;

namespace SeekMail.Models
{
    public class PostMessageModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}