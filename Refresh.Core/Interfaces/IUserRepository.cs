using Refresh.Core.Entities;

namespace Refresh.Core.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User?> GetUserById(int userId);
    Task<User?> GetUserByEmail(string email);
}