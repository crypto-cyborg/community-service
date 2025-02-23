using System.Text;
using System.Text.Json;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.RabbitMq;
using CommunityService.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunityService.Infrastructure.Services;

public class ReactionBackgroundService(IServiceScopeFactory factory, Consumer consumer) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.AddListener("reaction-queue", async (_, args) =>
        {
            await using var scope = factory.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<CommunityContext>();

            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var reaction = JsonSerializer.Deserialize<Reaction>(json) ?? throw new Exception("Cannot deserialize");

            Console.WriteLine("Saved one");
            
            await context.Reactions.AddAsync(reaction, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
        });
        
        return Task.CompletedTask;
    }
}