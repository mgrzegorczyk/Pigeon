using System.ComponentModel.DataAnnotations;

namespace Pigeon.Infrastructure.Options;

public class ProducerOptions
{
    [Required(ErrorMessage = "Producer chat message receive topic is required.")]
    public string ChatMessageReceiveTopic { get; init; }
}