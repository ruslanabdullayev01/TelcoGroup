using TelcoGroup.Models;

namespace TelcoGroup.ViewModels
{
    public class HomeVM
    {
        public Appeal? Appeal { get; set; }
        public List<Service>? Services { get; set; }
        public List<Solution>? Solutions { get; set; }
        public News? MainNews { get; set; }
        public List<News>? LatestNews { get; set; }
        public List<PartnerSlider>? PartnerSliders { get; set; }
        public AboutUs? AboutUs { get; set; }
    }
}
