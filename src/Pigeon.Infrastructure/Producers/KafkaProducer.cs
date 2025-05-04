using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pigeon.Infrastructure.Options;

namespace Pigeon.Infrastructure.Producers;

public interface IKafkaProducer
{
    Task ProduceAsync(string topic, Message<string, string> message);
}

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IOptions<KafkaOptions> _kafkaOptions;

    public KafkaProducer(ILogger<KafkaProducer> logger, IOptions<KafkaOptions> kafkaOptions)
    {
        _logger = logger;
        _kafkaOptions = kafkaOptions;
        
        var config = new ProducerConfig()
        {
            BootstrapServers = _kafkaOptions.Value.Url,
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, Message<string, string> message)
    {
        try
        {
            await _producer.ProduceAsync(topic, message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw new Exception("Can not produce message!", e);
        }
    }
}