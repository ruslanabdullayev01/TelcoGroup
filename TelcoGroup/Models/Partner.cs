using System.ComponentModel.DataAnnotations.Schema;

namespace TelcoGroup.Models
{
    public class Partner:BaseEntity
    {
        public string? ImagePath { get; set; }
        [NotMapped] public IFormFile? Photo { get; set; }
        public string? Link { get; set; }
    }
}
