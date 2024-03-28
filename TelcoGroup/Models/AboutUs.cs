namespace TelcoGroup.Models
{
    public class AboutUs:BaseEntity
    {
        public string TopDescription { get; set; }
        public string MiddleDescription { get; set; }
        public string BottomDescription { get; set; }
        public string GeneralDescription { get; set; }
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public int LanguageGroup { get; set; }
    }
}
