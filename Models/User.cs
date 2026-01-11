using System.ComponentModel.DataAnnotations;

namespace InternshipApplicationBackend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        //[StringLength(32, ErrorMessage = "Email Address can not exceed 32 characters.")]
        public string Email { get; set; }

        // Password must include at least 8 characters with one uppercase letter, 
        // one lowercase letter, one digit and one special character. e.g. : Abcdef123@
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "Password must include at least 8 characters with one uppercase letter, one lowercase letter, one digit and one special character.")]
        // [StringLength(100, ErrorMessage = "Password can not exceed 50 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        [StringLength(48, ErrorMessage = "User name can not exceed 48 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Mobile number must be 10 digits.")]
        [StringLength(10, ErrorMessage = "Mobile Number can not exceed 10 characters.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "User Role is required.")]
        [StringLength(8, ErrorMessage = "User Role can not exceed 8 characters.")]
        public string UserRole { get; set; }

        // Required only during admin registration
        [StringLength(56, ErrorMessage = "Secret Key can not exceed 56 characters.")]
        public string? SecretKey { get; set; }
    }
}
