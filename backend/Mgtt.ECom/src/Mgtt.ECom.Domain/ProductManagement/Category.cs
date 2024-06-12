using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.ProductManagement
{
    /// <summary>
    /// Represents a category in the e-commerce system.
    /// </summary>
    public class Category : IValidatableObject
    {
        public int CategoryID { get; internal set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Validates the properties of the Category object.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult($"{nameof(Name)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
