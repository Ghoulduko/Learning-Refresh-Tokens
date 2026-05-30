using Microsoft.EntityFrameworkCore;
using Refresh.Core.Entities;

namespace Refresh.Core.Database;

public class TokenDbContext : DbContext
{
    public TokenDbContext(DbContextOptions<TokenDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}