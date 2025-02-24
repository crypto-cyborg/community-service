using System.Text;
using System.Text.Json;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.Models;
using CommunityService.Infrastructure.RabbitMq;
using CommunityService.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunityService.Infrastructure.Services;

public class ReactionBackgroundService(IServiceScopeFactory factory, Consumer consumer)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.AddListener("reaction-queue", async (_, args) =>
        {
            try
            {
                await using var scope = factory.CreateAsyncScope();

                var context = scope.ServiceProvider.GetRequiredService<CommunityContext>();

                var body = args.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<ReactionMessage>(json) ??
                              throw new Exception("Cannot deserialize");

                switch (message.Action)
                {
                    case ReactAction.Add:
                    {
                        await context.Reactions.AddAsync(message.Reaction, stoppingToken);
                        break;
                    }
                    case ReactAction.Remove:
                    {
                        context.Reactions.Remove(message.Reaction);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(message.Action.ToString());
                }

                await context.SaveChangesAsync(stoppingToken);
            }
            catch (Exception e)
            {
                // TODO: Maybe some day...
                // logger.Log();
                Console.WriteLine($"[{DateTimeOffset.Now}]\t{e.GetType().Name}\t{e.Message}");
            }
        });

        return Task.CompletedTask;
    }
}