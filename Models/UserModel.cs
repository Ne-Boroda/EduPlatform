using Microsoft.AspNetCore.Identity;

namespace EduPlatform.Models
{
    public class UserModel : IdentityUser
    {
        public string login { get; set; }
        public string password { get; set; }
    }
}
