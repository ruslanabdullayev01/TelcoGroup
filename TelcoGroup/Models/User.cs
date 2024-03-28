using Microsoft.AspNetCore.Identity;

namespace TelcoGroup.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
