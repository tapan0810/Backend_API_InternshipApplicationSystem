using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InternshipApplicationBackend.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        [ForeignKey("User")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Feedback Text is required.")]
        [StringLength(1012, ErrorMessage = "Feedback Text can not exceed 1012 characters.")]
        public string? FeedbackText { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Date format should be yyyy-MM-dd.")]
        // [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User? User { get; set; }
    }
}


