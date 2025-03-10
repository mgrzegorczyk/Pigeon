namespace Pigeon.Application.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public string MessageText { get; set; }
    public DateTime CreatedAt { get; set; }
}