using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.ReviewManagement
{
    /// <summary>
    /// Represents a product review in the e-commerce system.
    /// </summary>
    public class Review : IValidatableObject
    {
        public int ReviewID { get; internal set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int UserID { get; set; }

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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Rating < 1 || Rating > 5)
            {
                yield return new ValidationResult($"{nameof(Rating)} must be between 1 and 5");
            }

            if (ReviewDate == default(DateTime))
            {
                yield return new ValidationResult($"{nameof(ReviewDate)} can't be default value");
            }

            yield return ValidationResult.Success;
        }
    }
}
