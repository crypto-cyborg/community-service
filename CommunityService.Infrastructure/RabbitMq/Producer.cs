using RabbitMQ.Client;

namespace CommunityService.Infrastructure.RabbitMq;

public class Producer(ConnectionFactory factory) : MqBase(factory)
{
    public async Task Send(string qName, byte[] message)
    {
        await CreateQueueIfNotExists(qName);
        await Channel.BasicPublishAsync(exchange: string.Empty, routingKey: qName, body: message);
    }
}