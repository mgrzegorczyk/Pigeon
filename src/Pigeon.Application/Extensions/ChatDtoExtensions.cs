using Pigeon.Application.DTOs;
using Pigeon.Domain.Models;

namespace Pigeon.Application.Extensions;

public static class ChatDtoExtensions
{
    public static ChatDto ToChatDto(this Chat chat)
    {
        return new ChatDto()
        {
            Id = chat.Id,
            CreatedAt = chat.CreatedAt,
            Messages = chat.Messages?.Select(x => new MessageDto(x.Id,
                x.ChatId,
                x.UserId,
                x.MessageText,
                x.CreatedAt)
            ).ToHashSet(),
            Name = chat.Name,
        };
    }
}