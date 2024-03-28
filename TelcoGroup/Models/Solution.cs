namespace TelcoGroup.Models
{
    public class Solution:BaseEntity
    {
        public string Title { get; set; }
        public string? SubTitle { get; set; } = string.Empty;
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public int LanguageGroup { get; set; }
    }
}
