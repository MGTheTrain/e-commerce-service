using Mgtt.ECom.Domain.UserManagement;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class UserService : IUserService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(PsqlDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            _logger.LogInformation("Fetching user by ID: {UserId}", userId);
            try
            {
                return await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            _logger.LogInformation("Fetching user by email: {Email}", email);
            try
            {
                return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by email: {Email}", email);
                throw;
            }
        }

        public async Task CreateUser(User user)
        {
            _logger.LogInformation("Creating new user: {UserName}, {Email}", user.UserName, user.Email);
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User created successfully: {UserId}", user.UserID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {UserName}, {Email}", user.UserName, user.Email);
                throw;
            }
        }

        public async Task UpdateUser(User user)
        {
            _logger.LogInformation("Updating user: {UserId}", user.UserID);
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User updated successfully: {UserId}", user.UserID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", user.UserID);
                throw;
            }
        }

        public async Task DeleteUser(Guid userId)
        {
            _logger.LogInformation("Deleting user: {UserId}", userId);
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User deleted successfully: {UserId}", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            _logger.LogInformation("Validating user: {Email}", email);
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning("User not found for validation: {Email}", email);
                    return false;
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                if (!isValid)
                {
                    _logger.LogWarning("Invalid password for user: {Email}", email);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user: {Email}", email);
                throw;
            }
        }
    }
}
