using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace InternshipApplicationBackend.Models
{
    [Index(nameof(DegreeProgram), Name = "Idx_InternshipApplication_DegreeProgram")]    // Apply index directly on DegreeProgram
    public class InternshipApplication
    {
        [Key]
        public int InternshipApplicationId { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Internship Id is required.")]
        [ForeignKey("Internship")]
        public int InternshipId { get; set; }

        [Required(ErrorMessage = "University Name is required.")]
        [StringLength(128, ErrorMessage = "University name can not exceed 128 characters.")]
        public string UniversityName { get; set; }

        [Required(ErrorMessage = "Degree Program is required.")]
        [StringLength(32, ErrorMessage = "Degree program can not exceed 32 characters.")]
        public string DegreeProgram { get; set; }

        [Required(ErrorMessage = "Resume is required.")]
        // [StringLength(1024, ErrorMessage = "Resume can not exceed 1024 characters.")]
        public string Resume { get; set; }  // File Name

        // [Url(ErrorMessage ="Enter a valid URL.")]
        [StringLength(512, ErrorMessage = "LinkedIn Profile can not exceed 512 characters.")]
        public string? LinkedInProfile { get; set; }

        [Required(ErrorMessage = "Application Status is required.")]
        [StringLength(8, ErrorMessage = "Applicaton Status can not exceed 8 characters.")]
        public string ApplicationStatus { get; set; }

        [Required(ErrorMessage = "Application Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Date format should be yyyy-MM-dd.")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ApplicationDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User? User { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Internship? Internship { get; set; }
    }
}