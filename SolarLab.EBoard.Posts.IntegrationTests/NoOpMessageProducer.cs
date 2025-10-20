using SolarLab.EBoard.Posts.Application.Abstractions.Messaging;

namespace SolarLab.EBoard.Posts.IntegrationTests;

public class NoOpMessageProducer : IMessageProducer
{
    public Task SendAsync(object message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}