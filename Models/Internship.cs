using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace InternshipApplicationBackend.Models
{
    [Index(nameof(CompanyName), Name = "Idx_Internship_CompanyName")]    // Apply index directly on CompanyName
    [Index(nameof(Location), Name = "Idx_Internship_Location")]    // Apply index directly on Location
    public class Internship
    {
        [Key]
        public int InternshipId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(128, ErrorMessage = "Title cannot exceed 128 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(128, ErrorMessage = "Company name cannot exceed 128 characters.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(128, ErrorMessage = "Location cannot exceed 128 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least one month or more.")]
        public int DurationInMonths { get; set; }

        [Required(ErrorMessage = "Stipend is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Stipend must be a non-negative value.")]
        public decimal Stipend { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1024, ErrorMessage = "Description cannot exceed 1024 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Skills Required is required.")]
        [StringLength(200, ErrorMessage = "Skills Required cannot exceed 200 characters.")]
        public string SkillsRequired { get; set; }

        [Required(ErrorMessage = "Application Deadline is required.")]
        [DataType(DataType.Date, ErrorMessage = "Date format should be yyyy-MM-dd.")]
        // [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [StringLength(16, ErrorMessage = "Application Deadline cannot exceed 16 characters.")]
        public string ApplicationDeadline { get; set; }
    }
}