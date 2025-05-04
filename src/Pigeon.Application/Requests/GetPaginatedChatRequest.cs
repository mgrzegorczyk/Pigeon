namespace Pigeon.Application.Requests;

public record GetPaginatedChatRequest(Guid ChatId, int PageNumber, int PageSize);