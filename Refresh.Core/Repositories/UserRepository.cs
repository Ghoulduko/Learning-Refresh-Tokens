using Microsoft.EntityFrameworkCore;
using Refresh.Core.Database;
using Refresh.Core.Entities;
using Refresh.Core.Interfaces;

namespace Refresh.Core.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TokenDbContext _context;
    public UserRepository(TokenDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}