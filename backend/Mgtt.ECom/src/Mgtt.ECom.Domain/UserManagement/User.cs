// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.UserManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a user in the e-commerce system.
    /// </summary>
    public class User : IValidatableObject
    {
        public User()
        {
            this.UserID = Guid.NewGuid();
            this.UserName = string.Empty;
            this.PasswordHash = string.Empty;
            this.Email = string.Empty;
            this.Role = string.Empty;
        }

        [Required]
        public Guid UserID { get; internal set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        /// <summary>
        /// Validates the properties of the User object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.UserID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(this.UserID)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.UserName))
            {
                yield return new ValidationResult($"{nameof(this.UserName)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.PasswordHash))
            {
                yield return new ValidationResult($"{nameof(this.PasswordHash)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                yield return new ValidationResult($"{nameof(this.Email)} can't be empty");
            }

            if (string.IsNullOrEmpty(this.Role))
            {
                yield return new ValidationResult($"{nameof(this.Role)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
