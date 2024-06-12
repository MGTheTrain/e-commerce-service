using Mgtt.ECom.Domain.UserManagement;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mgtt.ECom.Application.Services.Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly PsqlDbContext _context;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<PsqlDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging() // Enable sensitive data logging to see entity key values
                .Options;

            _context = new PsqlDbContext(options);

            // Initialize any required properties like Role in the in-memory context
            _context.Users.Add(new User { UserName = "TestUser", Email = "testuser@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "User" });
            _context.SaveChanges();

            _userService = new UserService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task Test_CreateUser_GetUserById()
        {
            // Arrange
            var user = new User { UserName = "TestUser2", Email = "testuser2@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };

            // Act
            await _userService.CreateUser(user);
            var result = await _userService.GetUserById(user.UserID);

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
            await _userService.CreateUser(user);

            // Act
            user.UserName = "UpdatedUser";
            await _userService.UpdateUser(user);
            var updatedUser = await _userService.GetUserById(user.UserID);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("UpdatedUser", updatedUser.UserName);
        }

        [Fact]
        public async Task Test_DeleteUser()
        {
            // Arrange
            var user = new User { UserName = "TestUser4", Email = "testuser4@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            await _userService.CreateUser(user);

            // Act
            await _userService.DeleteUser(user.UserID);
            var deletedUser = await _userService.GetUserById(user.UserID);

            // Assert
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task Test_ValidateUser()
        {
            // Arrange
            var user = new User { UserName = "TestUser5", Email = "testuser5@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            await _userService.CreateUser(user);

            // Act
            var isValid = await _userService.ValidateUser("testuser5@example.com", "password");

            // Assert
            Assert.True(isValid);
        }
    }
}
