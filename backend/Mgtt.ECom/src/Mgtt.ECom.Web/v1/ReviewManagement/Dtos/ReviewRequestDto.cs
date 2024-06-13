using System;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Web.v1.ReviewManagement.DTOs
{
    public class ReviewRequestDTO
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }
    }
}
