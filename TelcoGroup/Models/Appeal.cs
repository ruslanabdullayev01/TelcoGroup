using System.ComponentModel.DataAnnotations;

namespace TelcoGroup.Models
{
    public class Appeal:BaseEntity
    {
        public string FullName { get; set; }
        [EmailAddress] public string Email { get; set; }
        [Phone] public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string? ReadedBy { get; set; }
        public bool Replied { get; set; }
    }
}
