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
            Messages = chat.Messages?.Select(x => new MessageDto()
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                MessageText = x.MessageText,
                ChatId = x.ChatId,
                UserId = x.UserId,
            }).ToHashSet(),
            Name = chat.Name,
        };
    }
}