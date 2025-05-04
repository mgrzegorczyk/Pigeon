using System.ComponentModel.DataAnnotations;

namespace Pigeon.Infrastructure.Options;

public class KafkaOptions
{
    [Required(ErrorMessage = "Kafka URL is required.")]
    public string Url { get; init; }
    
    [Required(ErrorMessage = "Producer is required.")]
    public ProducerOptions Producer { get; set; }
}