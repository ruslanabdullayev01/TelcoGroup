namespace TelcoGroup.Models
{
    public class Header
    {
        public int Id { get; set; }
        public string? BlueText { get; set; }
        public string? GreenText { get; set; }
        public string? PageKey { get; set; }
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public int LanguageGroup { get; set; }
    }
}
