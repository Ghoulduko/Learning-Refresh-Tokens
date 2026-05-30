using Microsoft.EntityFrameworkCore;
using Refresh.Core.Database;
using Refresh.Core.Entities;
using Refresh.Core.Interfaces;

namespace Refresh.Core.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly TokenDbContext _context;
    public RefreshTokenRepository(TokenDbContext context)
    {
        _context = context;
    }

    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokens()
    {
        return await _context.RefreshTokens.ToListAsync();
    }

    public async Task<RefreshToken?> GetRefreshToken(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task Delete(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
    }
}