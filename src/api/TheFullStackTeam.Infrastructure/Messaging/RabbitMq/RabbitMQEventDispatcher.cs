using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;
using TheFullStackTeam.Common.Configuration;
using TheFullStackTeam.Common.Converters;
using TheFullStackTeam.Domain.Events;
using TheFullStackTeam.Domain.Services;

namespace TheFullStackTeam.Infrastructure.Messaging.RabbitMq;

public class RabbitMQEventDispatcher : IEventDispatcher
{
    private readonly Lazy<IConnection> _connection;
    private readonly Lazy<IModel> _channel;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQEventDispatcher> _logger;

    public RabbitMQEventDispatcher(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQEventDispatcher> logger)
    {
        _logger = logger;
        _settings = settings.Value;
        _connection = new Lazy<IConnection>(() =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            return factory.CreateConnection();
        });
        _channel = new Lazy<IModel>(() =>
        {
            // Create the channel from the connection
            var channel = _connection.Value.CreateModel();

            // Declare the exchange
            channel.ExchangeDeclare(
                exchange: _settings.ExchangeName,
                type: "fanout",  // Use "fanout" to broadcast the message to multiple queues
                durable: true,
                autoDelete: false,
                arguments: null
            );
            return channel;
            // NOTE: bind queues to the exchange in the consumer side
        });
    }

    public Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : EventBase
    {
        try
        {
            // Ensure the channel is initialized
            var channel = _channel.Value;

            // Include the assembly name to deserialize the event
            var eventType = domainEvent.GetType().AssemblyQualifiedName;
            var messagePayload = new
            {
                EventType = eventType,
                EventData = domainEvent
            };
            var jsonOptions = new JsonSerializerOptions
            {
                Converters = { new UlidJsonConverter() }
            };
            var message = JsonSerializer.Serialize(messagePayload, jsonOptions);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = new Dictionary<string, object>
            {
                { "CorrelationId", domainEvent.CorrelationId }
            };

            channel.BasicPublish(
                exchange: _settings.ExchangeName,
                routingKey: "",
                basicProperties: properties,
                body: body
            );
        }
        catch (BrokerUnreachableException ex)
        {
            _logger.LogError(ex, $"RabbitMQ is unreachable. Event Type: {domainEvent.GetType().Name}");
        }
        catch (AlreadyClosedException ex)
        {
            _logger.LogError(ex, $"RabbitMQ connection is closed. Event Type: {domainEvent.GetType().Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred when sending event to RabbitMQ. Event Type: {domainEvent.GetType().Name}");
        }
        return Task.CompletedTask;
    }

}
