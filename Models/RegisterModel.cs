using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool isStudent { get; set; }

        [Required]
        public bool isTeacher {  get; set; }
    }
}
