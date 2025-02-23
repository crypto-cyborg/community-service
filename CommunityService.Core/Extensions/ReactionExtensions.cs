using CommunityService.Core.Models;

namespace CommunityService.Core.Extensions;

public static class ReactionExtensions
{
    public record ReactionReadDto(Guid Id, Guid UserId, string PostId, ReactionType Type, DateTimeOffset Time);

    public static ReactionReadDto MapToResponse(this Reaction r) =>
        new(r.Id, r.UserId, r.PostId, r.Type, r.Time);

    public static IEnumerable<ReactionReadDto> MapToResponse(this IEnumerable<Reaction> reactions) =>
        reactions.Select(r => r.MapToResponse());
}