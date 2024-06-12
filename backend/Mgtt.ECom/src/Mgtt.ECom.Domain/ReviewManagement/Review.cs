using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mgtt.ECom.Domain.UserManagement;

namespace Mgtt.ECom.Domain.ReviewManagement
{
    /// <summary>
    /// Represents a product review in the e-commerce system.
    /// </summary>
    public class Review : IValidatableObject
    {
        public Review()
        {
            ReviewID = Guid.NewGuid();
            ProductID = Guid.Empty;
            UserID = Guid.Empty;
            Rating = 3;
            Comment = string.Empty;
            ReviewDate = DateTime.UtcNow;
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReviewID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(ReviewID)} can't be empty");
            }

            if (ProductID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(ProductID)} can't be empty");
            }

            if (UserID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(UserID)} can't be empty");
            }

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
