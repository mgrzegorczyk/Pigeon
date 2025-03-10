using Pigeon.Application.DTOs;
using Pigeon.Application.Extensions;
using Pigeon.Domain.Exceptions;
using Pigeon.Domain.Interfaces.Repositories;

namespace Pigeon.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<ChatDto> GetPaginatedChat(Guid chatId, int pageNumber, int pageSize)
    {
        var paginedChat = await _chatRepository.GetByIdAsync(chatId, pageNumber, pageSize);

        if (paginedChat == null)
        {
            throw new ChatNotFoundException(chatId);
        }
        
        return paginedChat.ToChatDto();
    }
}