using Microsoft.EntityFrameworkCore;
using Pigeon.Domain.Models;

namespace Pigeon.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasOne(x => x.Chat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId);

        modelBuilder.Entity<Message>()
            .HasOne(x => x.User)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<Chat>()
            .HasData(new Chat() { Id = Guid.NewGuid(), Name = "Main", CreatedAt = DateTime.Now });
    }
}