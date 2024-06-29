// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.UserManagement;

public interface IUserService
{
    Task<IEnumerable<User>?> GetAllUsers();

    Task<User?> GetUserById(Guid userId);

    Task<User?> GetUserByEmail(string email);

    Task<User?> CreateUser(User user);

    Task<User?> UpdateUser(User user);

    Task DeleteUser(Guid userId);

    Task<bool> ValidateUser(string email, string password);
}
