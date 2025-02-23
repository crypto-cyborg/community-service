using RabbitMQ.Client;

namespace CommunityService.Infrastructure.RabbitMq;

public class MqBase
{
    private readonly IConnection _connection;
    protected readonly IChannel Channel;

    public MqBase(ConnectionFactory factory)
    {
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        Channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    protected Task CreateQueueIfNotExists(string qName) =>
        Channel.QueueDeclareAsync(qName, autoDelete: false, exclusive: false, durable: true);

    ~MqBase()
    {
        _connection.Dispose();
        Channel.Dispose();
    }
}