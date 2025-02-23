using System.Text;
using System.Text.Json;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.RabbitMq;
using CommunityService.Persistence;
using LanguageExt;

namespace CommunityService.Application.Services;

public class ReactionService(UnitOfWork unitOfWork, Producer producer) : IReactionService
{
    public async Task<Fin<Reaction>> React(string postId, Guid userId, int reactionType)
    {
        var post = await unitOfWork.PostsRepository.GetByIdAsync(postId);
        if (post is null) return Fin<Reaction>.Fail("Required post does not exists");

        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user is null) return Fin<Reaction>.Fail("Required user does not exist");

        var reaction = new Reaction
        {
            UserId = userId,
            PostId = postId,
            TypeId = reactionType
        };

        var json = JsonSerializer.Serialize(reaction);
        var bytes = Encoding.UTF8.GetBytes(json);
        await producer.Send("reaction-queue", bytes);

        return reaction;
    }

    public async Task<Fin<Reaction>> Undo(string postId, Guid userId, int reactionType)
    {
        var post = await unitOfWork.PostsRepository.GetByIdAsync(postId);
        if (post is null) return Fin<Reaction>.Fail("post");

        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user is null) return Fin<Reaction>.Fail("user");

        var reaction = post.Reactions.FirstOrDefault(r => r.UserId == userId && r.TypeId == reactionType);

        if (reaction is null) return Fin<Reaction>.Fail("reaction");

        post.Reactions.Remove(reaction);
        await unitOfWork.SaveCommunityChangesAsync();

        return reaction;
    }
}