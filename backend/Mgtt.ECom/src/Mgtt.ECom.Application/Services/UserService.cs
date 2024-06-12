using Mgtt.ECom.Domain.UserManagement;
using Mgtt.ECom.Persistence.DataAccess;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class UserService : IUserService
    {
        private readonly PsqlDbContext _context;

        public UserService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await Task.FromResult(_context.Users.Find(userId));
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await Task.FromResult(_context.Users.SingleOrDefault(u => u.Email == email));
        }

        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user == null) return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
