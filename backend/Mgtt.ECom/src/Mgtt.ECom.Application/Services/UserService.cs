// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.UserManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class UserService : IUserService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<UserService> logger;

        public UserService(PsqlDbContext context, ILogger<UserService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            this.logger.LogInformation("Fetching user by ID: {UserId}", userId);
            try
            {
                return await this.context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching user by ID: {UserId}", userId);
                return await Task.FromResult<User?>(null);
            }
        }

        public async Task<IEnumerable<User>?> GetAllUsers()
        {
            this.logger.LogInformation("Fetching all users");
            try
            {
                return await this.context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching all users");
                return await Task.FromResult<IEnumerable<User>?>(null);
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            this.logger.LogInformation("Fetching user by email: {Email}", email);
            try
            {
                return await this.context.Users.SingleOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching user by email: {Email}", email);
                return await Task.FromResult<User?>(null);
            }
        }

        public async Task<User?> CreateUser(User user)
        {
            this.logger.LogInformation("Creating new user: {UserName}, {Email}", user.UserName, user.Email);
            try
            {
                var existingUser = await this.context.Users
                                                 .FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    this.logger.LogWarning("User with email {Email} already exists.", user.Email);
                    return await Task.FromResult<User?>(null);
                }

                this.context.Users.Add(user);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("User created successfully: {UserId}", user.UserID);
                return await Task.FromResult<User?>(user);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating user: {UserName}, {Email}", user.UserName, user.Email);
                return await Task.FromResult<User?>(null);
            }
        }

        public async Task<User?> UpdateUser(User user)
        {
            this.logger.LogInformation("Updating user: {UserId}", user.UserID);
            try
            {
                this.context.Users.Update(user);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("User updated successfully: {UserId}", user.UserID);
                return await Task.FromResult<User?>(user);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating user: {UserId}", user.UserID);
                return await Task.FromResult<User?>(null);
            }
        }

        public async Task DeleteUser(Guid userId)
        {
            this.logger.LogInformation("Deleting user: {UserId}", userId);
            try
            {
                var user = await this.context.Users.FindAsync(userId);
                if (user != null)
                {
                    this.context.Users.Remove(user);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("User deleted successfully: {UserId}", userId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting user: {UserId}", userId);
            }
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            this.logger.LogInformation("Validating user: {Email}", email);
            try
            {
                var user = await this.context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    this.logger.LogWarning("User not found for validation: {Email}", email);
                    return false;
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                if (!isValid)
                {
                    this.logger.LogWarning("Invalid password for user: {Email}", email);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error validating user: {Email}", email);
                return false;
            }
        }
    }
}
