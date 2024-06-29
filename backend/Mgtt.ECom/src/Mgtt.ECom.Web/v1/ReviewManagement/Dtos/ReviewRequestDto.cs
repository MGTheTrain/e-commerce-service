// <copyright file="ReviewRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ReviewManagement.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

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
