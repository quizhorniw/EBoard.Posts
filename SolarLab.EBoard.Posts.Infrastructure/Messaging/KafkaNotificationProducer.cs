using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SolarLab.EBoard.Posts.Application.Abstractions.Messaging;

namespace SolarLab.EBoard.Posts.Infrastructure.Messaging;

public class KafkaNotificationProducer : IMessageProducer
{
    private readonly ILogger<KafkaNotificationProducer> _logger;
    private readonly IProducer<string, string> _producer;
    private const string Topic = "notifications";

    public KafkaNotificationProducer(ILogger<KafkaNotificationProducer> logger, IProducer<string, string> producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task SendAsync(object message, CancellationToken cancellationToken = default)
    {
        try
        {
            var kafkaMessage = new Message<string, string>
            {
                Value = JsonConvert.SerializeObject(message)
            };

            _logger.LogInformation("Kafka message: {Message}", kafkaMessage.Value);
            
            await _producer.ProduceAsync(Topic, kafkaMessage, cancellationToken);
        }
        catch (ProduceException<string, string> e)
        {
            throw new ApplicationException("Error while producing notification message", e);
        }
    }
}