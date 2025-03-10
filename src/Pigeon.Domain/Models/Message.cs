namespace Pigeon.Domain.Models;

public class Message
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public string MessageText { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Chat Chat { get; set; }
    public User User { get; set; }
}