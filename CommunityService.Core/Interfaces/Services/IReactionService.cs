using CommunityService.Core.Models;
using LanguageExt;

namespace CommunityService.Core.Interfaces.Services;

public interface IReactionService
{
    Task<Fin<Reaction>> React(string postId, Guid userId, int reactionType);
    Task<Fin<Reaction>> Undo(string postId, Guid userId, int reactionType);
}