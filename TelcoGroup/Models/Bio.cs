using System.ComponentModel.DataAnnotations;

namespace TelcoGroup.Models
{
    public class Bio:BaseEntity
    {
        [Phone] public string? PhoneNumber { get; set; }
        [EmailAddress] public string? Email { get; set; }
        public string Address { get; set; }
        public string? Facebook { get; set; }
        public string? Linkedin { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? MapLink { get; set; }
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public int LanguageGroup { get; set; }
    }
}
