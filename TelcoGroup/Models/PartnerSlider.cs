using System.ComponentModel.DataAnnotations.Schema;

namespace TelcoGroup.Models
{
    public class PartnerSlider:BaseEntity
    {
        public string? ImagePath { get; set; }
        [NotMapped] public IFormFile? Photo { get; set; }
    }
}
