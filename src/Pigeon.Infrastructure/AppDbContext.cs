using Microsoft.EntityFrameworkCore;
using Pigeon.Domain;

namespace Pigeon.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    {
    }

    public DbSet<User> Users { get; set; }
}