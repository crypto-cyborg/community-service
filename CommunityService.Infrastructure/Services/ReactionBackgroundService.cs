using System.Text;
using System.Text.Json;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.Models;
using CommunityService.Infrastructure.RabbitMq;
using CommunityService.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
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

                var communityContext = scope.ServiceProvider.GetRequiredService<CommunityContext>();
                var forumContext = scope.ServiceProvider.GetRequiredService<ForumContext>();

                var body = args.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<ReactionMessage>(json) ??
                              throw new Exception("Cannot deserialize");

                var post = await forumContext.Posts.FirstOrDefaultAsync(p => p.Id == message.Reaction.PostId,
                               stoppingToken)
                           ?? throw new InvalidOperationException("Cannot find post to react");

                switch (message.Action)
                {
                    case ReactAction.Add:
                    {
                        await communityContext.Reactions.AddAsync(message.Reaction, stoppingToken);
                        post.ReactionsCount += 1;
                        if (message.Reaction.TypeId == 1) post.LikesCount += 1;
                        break;
                    }
                    case ReactAction.Remove:
                    {
                        communityContext.Reactions.Remove(message.Reaction);
                        post.ReactionsCount -= 1;
                        if (message.Reaction.TypeId == 1) post.LikesCount -= 1;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(message.Action.ToString());
                }

                await communityContext.SaveChangesAsync(stoppingToken);
                await forumContext.SaveChangesAsync(stoppingToken);
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