using Microsoft.AspNetCore.Identity;

namespace D2LExtensionWebAPPSSR.Models
{
    public class User : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
