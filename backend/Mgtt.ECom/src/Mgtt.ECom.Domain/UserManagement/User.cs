using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.UserManagement
{
    /// <summary>
    /// Represents a user in the e-commerce system.
    /// </summary>
    public class User : IValidatableObject
    {
        public User()
        {
            UserID = Guid.NewGuid();
            UserName = string.Empty;
            PasswordHash = string.Empty;
            Email = string.Empty;
            Role = string.Empty;
        }

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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(UserID)} can't be empty");
            }

            if (string.IsNullOrEmpty(UserName))
            {
                yield return new ValidationResult($"{nameof(UserName)} can't be empty");
            }

            if (string.IsNullOrEmpty(PasswordHash))
            {
                yield return new ValidationResult($"{nameof(PasswordHash)} can't be empty");
            }

            if (string.IsNullOrEmpty(Email))
            {
                yield return new ValidationResult($"{nameof(Email)} can't be empty");
            }

            if (string.IsNullOrEmpty(Role))
            {
                yield return new ValidationResult($"{nameof(Role)} can't be empty");
            }

            yield return ValidationResult.Success;
        }
    }
}
