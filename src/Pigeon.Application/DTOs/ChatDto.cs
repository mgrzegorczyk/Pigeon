namespace Pigeon.Application.DTOs;

public class ChatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public HashSet<MessageDto>? Messages { get; set; }
}