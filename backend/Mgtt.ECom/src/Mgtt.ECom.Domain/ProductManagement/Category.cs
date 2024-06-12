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
        public Category()
        {
            CategoryID = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
        }

        public Guid CategoryID { get; internal set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Validates the properties of the Category object.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CategoryID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(CategoryID)} can't be empty");
            }

            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult($"{nameof(Name)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
