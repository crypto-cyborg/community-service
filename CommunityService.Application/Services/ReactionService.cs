using System.Text;
using System.Text.Json;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.Models;
using CommunityService.Infrastructure.RabbitMq;
using CommunityService.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class ReactionService(UnitOfWork unitOfWork, Producer producer) : IReactionService
{
    public async Task<Fin<Reaction>> React(string postId, Guid userId, int reactionType)
    {
        var post = await unitOfWork.PostsRepository.GetByIdAsync(postId);
        if (post is null) return Fin<Reaction>.Fail("Required post does not exists");

        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user is null) return Fin<Reaction>.Fail("Required user does not exist");

        var reaction = ReactionExists(postId, userId, reactionType);
        if (reaction is not null) return Fin<Reaction>.Succ(reaction);

        reaction = new Reaction
        {
            UserId = userId,
            PostId = postId,
            TypeId = reactionType
        };

        var message = new ReactionMessage
        {
            Reaction = reaction,
            Action = ReactAction.Add
        };

        await SendMessage(message);

        return reaction;
    }

    public async Task<Fin<Reaction>> Undo(string postId, Guid userId, int reactionType)
    {
        var post = await unitOfWork.PostsRepository.GetByIdAsync(postId);
        if (post is null) return Fin<Reaction>.Fail("Required post does not exist");

        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user is null) return Fin<Reaction>.Fail("Required user does not exist");

        // TODO: Rewrite repository to auto-include props
        post.Reactions = await unitOfWork.ReactionRepository.GetAsync(r => r.PostId == postId).ToListAsync();

        var reaction = post.Reactions.FirstOrDefault(r => r.UserId == userId && r.TypeId == reactionType);

        if (reaction is null) return Fin<Reaction>.Fail("Cannot undo. Reaction is null");

        var message = new ReactionMessage
        {
            Reaction = reaction,
            Action = ReactAction.Remove
        };

        await SendMessage(message);

        return reaction;
    }

    public async Task<Fin<IEnumerable<ReactionType>>> GetAvailableTypes()
    {
        var types = await unitOfWork.ReactionTypesRepository.GetAsync().AsNoTracking().ToListAsync();

        return types;
    }

    private Reaction? ReactionExists(string postId, Guid userId, int reactionTypeId)
    {
        return unitOfWork.ReactionRepository.GetAsync(r =>
            r.PostId == postId && r.UserId == userId && r.TypeId == reactionTypeId).FirstOrDefault();
    }

    private async Task SendMessage(ReactionMessage message)
    {
        var json = JsonSerializer.Serialize(message);
        var bytes = Encoding.UTF8.GetBytes(json);
        await producer.Send("reaction-queue", bytes);
    }
}