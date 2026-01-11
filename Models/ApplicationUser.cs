using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InternshipApplicationBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(30, ErrorMessage = "Name can not exceed 30 characters.")]
        public string Name { get; set; }
    }
}