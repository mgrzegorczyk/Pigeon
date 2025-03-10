using Microsoft.EntityFrameworkCore;
using Pigeon.Domain.Interfaces.Repositories;
using Pigeon.Domain.Models;

namespace Pigeon.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _dbContext;

    public ChatRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Chat?> GetByIdAsync(Guid id, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) throw new ArgumentException("Page number cannot be less than 1");
        if (pageSize < 1) throw new ArgumentException("Page size cannot be less than 1");

        var chat = await _dbContext.Set<Chat>()
            .Where(x => x.Id == id)
            .Include(x => x.Messages)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .FirstOrDefaultAsync();

        return chat;
    }
}