using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Mgtt.ECom.DomainTest.User
{
    public class UserTests
    {
        [Fact]
        public void User_Validation_Succeeds_With_Valid_Data()
        {
            // Arrange
            var user = new Mgtt.ECom.Domain.UserManagement.User
            {
                UserName = "JohnDoe",
                PasswordHash = "hashedpassword",
                Email = "john.doe@example.com",
                Role = "Admin"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "hashedpassword", "john.doe@example.com", "Admin", "UserName")]
        [InlineData("JohnDoe", "", "john.doe@example.com", "Admin", "PasswordHash")]
        [InlineData("JohnDoe", "hashedpassword", "", "Admin", "Email")]
        [InlineData("JohnDoe", "hashedpassword", "john.doe@example.com", "", "Role")]
        public void User_Validation_Fails_With_Invalid_Data(string userName, string passwordHash, string email, string role, string expectedMemberName)
        {
            // Arrange
            var user = new Mgtt.ECom.Domain.UserManagement.User
            {
                UserName = userName,
                PasswordHash = passwordHash,
                Email = email,
                Role = role
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains(expectedMemberName));
        }
    }
}
