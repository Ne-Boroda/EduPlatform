using Microsoft.AspNetCore.Identity;

namespace EduPlatform.Models
{
    public class UserModel : IdentityUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
