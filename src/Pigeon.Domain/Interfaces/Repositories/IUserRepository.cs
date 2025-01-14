namespace Pigeon.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task DeleteAsync(Guid userId);
    Task UpdateAsync(User user);
}