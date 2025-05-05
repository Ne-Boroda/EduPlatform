using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool isTeacher { get; set; }
    }
}
