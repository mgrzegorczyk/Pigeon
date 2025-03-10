using Pigeon.Application.DTOs;

namespace Pigeon.Application.Services;

public interface IChatService
{
    Task<ChatDto> GetPaginatedChat(Guid chatId, int pageNumber, int pageSize);
}