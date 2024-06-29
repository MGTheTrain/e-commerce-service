// <copyright file="Review.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ReviewManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Mgtt.ECom.Domain.UserManagement;

    /// <summary>
    /// Represents a product review in the e-commerce system.
    /// </summary>
    public class Review : IValidatableObject
    {
        public Review()
        {
            this.ReviewID = Guid.NewGuid();
            this.ProductID = Guid.Empty;
            this.UserID = Guid.Empty;
            this.Rating = 3;
            this.Comment = string.Empty;
            this.ReviewDate = DateTime.UtcNow;
        }

        public Guid ReviewID { get; internal set; }

        [Required]
        public Guid ProductID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// Validates the properties of the Review object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ReviewID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.ReviewID)} can't be empty");
            }

            if (this.ProductID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.ProductID)} can't be empty");
            }

            if (this.UserID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.UserID)} can't be empty");
            }

            if (this.Rating < 1 || this.Rating > 5)
            {
                yield return new ValidationResult($"{nameof(this.Rating)} must be between 1 and 5");
            }

            if (this.ReviewDate == default(DateTime))
            {
                yield return new ValidationResult($"{nameof(this.ReviewDate)} can't be default value");
            }

            yield return ValidationResult.Success;
        }
    }
}
