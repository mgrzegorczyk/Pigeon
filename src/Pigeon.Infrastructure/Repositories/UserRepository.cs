using Microsoft.EntityFrameworkCore;
using Pigeon.Domain;
using Pigeon.Domain.Interfaces.Repositories;

namespace Pigeon.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        return user;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        return user;
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        var userToDelete = await GetByIdAsync(userId);
        if (userToDelete == null) return;

        _dbContext.Users.Remove(userToDelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}