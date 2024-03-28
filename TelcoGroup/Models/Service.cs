using System.ComponentModel.DataAnnotations.Schema;

namespace TelcoGroup.Models
{
    public class Service:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? IconPath { get; set; }
        [NotMapped] public IFormFile? Photo { get; set; }
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public int LanguageGroup { get; set; }
    }
}
