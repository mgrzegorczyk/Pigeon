using Pigeon.Domain.Models;

namespace Pigeon.Domain.Interfaces.Repositories;

public interface IChatRepository
{
    Task<Chat?> GetByIdAsync(Guid id, int pageNumber, int pageSize);
}