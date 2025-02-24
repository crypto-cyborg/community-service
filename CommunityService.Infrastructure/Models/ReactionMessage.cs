using CommunityService.Core.Models;

namespace CommunityService.Infrastructure.Models;

public class ReactionMessage
{
    public required Reaction Reaction { get; init; }
    public required ReactAction Action { get; init; }
}