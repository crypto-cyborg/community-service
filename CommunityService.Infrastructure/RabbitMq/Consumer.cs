using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommunityService.Infrastructure.RabbitMq;

public class Consumer : MqBase
{
    private readonly AsyncEventingBasicConsumer _consumer;

    public Consumer(ConnectionFactory factory) : base(factory)
    {
        _consumer = new AsyncEventingBasicConsumer(Channel);
    }

    public void AddListener(string qName, AsyncEventHandler<BasicDeliverEventArgs> handler)
    {
        _consumer.ReceivedAsync += handler;

        CreateQueueIfNotExists(qName);

        Channel.BasicConsumeAsync(queue: qName, autoAck: true, consumer: _consumer);
    }

    public void RemoveListener(AsyncEventHandler<BasicDeliverEventArgs> handler) => _consumer.ReceivedAsync -= handler;
}