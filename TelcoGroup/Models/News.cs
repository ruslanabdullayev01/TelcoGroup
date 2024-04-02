using System.ComponentModel.DataAnnotations.Schema;

namespace TelcoGroup.Models
{
    public class News : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped] public IFormFile? Photo { get; set; }
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public int LanguageGroup { get; set; }
    }
}
