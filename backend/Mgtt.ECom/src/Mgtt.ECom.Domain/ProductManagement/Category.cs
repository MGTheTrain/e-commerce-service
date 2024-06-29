// <copyright file="Category.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ProductManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a category in the e-commerce system.
    /// </summary>
    public class Category : IValidatableObject
    {
        public Category()
        {
            this.CategoryID = Guid.NewGuid();
            this.Name = string.Empty;
            this.Description = string.Empty;
        }

        public Guid CategoryID { get; internal set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Validates the properties of the Category object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CategoryID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.CategoryID)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                yield return new ValidationResult($"{nameof(this.Name)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
