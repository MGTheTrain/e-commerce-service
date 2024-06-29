// <copyright file="UserServiceTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.ApplicationTest.Services
{
    using System;
    using System.Threading.Tasks;
    using Mgtt.ECom.Application.Services;
    using Mgtt.ECom.Domain.UserManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class UserServiceTests : IDisposable
    {
        private readonly PsqlDbContext context;
        private readonly UserService userService;
        private readonly ILogger<UserService> logger;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<PsqlDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging() // Enable sensitive data logging to see entity key values
                .Options;

            this.context = new PsqlDbContext(options);

            // Initialize any required properties like Role in the in-memory context
            this.context.Users.Add(new User { UserName = "TestUser", Email = "testuser@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "User" });
            this.context.SaveChanges();

            // Mock ILogger
            var loggerMock = new Mock<ILogger<UserService>>();

            this.userService = new UserService(this.context, loggerMock.Object);
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        [Fact]
        public async Task Test_CreateUser_GetUserById()
        {
            // Arrange
            var user = new User { UserName = "TestUser2", Email = "testuser2@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };

            // Act
            await this.userService.CreateUser(user);
            var result = await this.userService.GetUserById(user.UserID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task Test_UpdateUser()
        {
            // Arrange
            var user = new User { UserName = "TestUser3", Email = "testuser3@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            await this.userService.CreateUser(user);

            // Act
            user.UserName = "UpdatedUser";
            await this.userService.UpdateUser(user);
            var updatedUser = await this.userService.GetUserById(user.UserID);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("UpdatedUser", updatedUser.UserName);
        }

        [Fact]
        public async Task Test_DeleteUser()
        {
            // Arrange
            var user = new User { UserName = "TestUser4", Email = "testuser4@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            await this.userService.CreateUser(user);

            // Act
            await this.userService.DeleteUser(user.UserID);
            var deletedUser = await this.userService.GetUserById(user.UserID);

            // Assert
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task Test_ValidateUser()
        {
            // Arrange
            var user = new User { UserName = "TestUser5", Email = "testuser5@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            await this.userService.CreateUser(user);

            // Act
            var isValid = await this.userService.ValidateUser("testuser5@example.com", "password");

            // Assert
            Assert.True(isValid);
        }
    }
}
