
namespace Mgtt.ECom.Domain.UserManagement;

public interface IUserService
{
    Task<User> GetUserById(int userId);
    Task<User> GetUserByEmail(string email);
    Task CreateUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(int userId);
    Task<bool> ValidateUser(string email, string password);
}
