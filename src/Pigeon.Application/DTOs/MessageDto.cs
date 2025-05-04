namespace Pigeon.Application.DTOs;

public record MessageDto(
    Guid Id,
    Guid ChatId,
    Guid UserId,
    string MessageText,
    DateTime CreatedAt
);