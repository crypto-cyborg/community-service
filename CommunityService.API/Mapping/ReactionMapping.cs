using CommunityService.Application.Models.Dto;
using CommunityService.Core.Models;

namespace CommunityService.API.Mapping;

public static class ReactionMapping
{
    public static ReactionReadDto MapToResponse(this Reaction r) =>
        new ReactionReadDto(r.Id, r.User.MapToResponse(), r.Type, r.Time);

    public static IEnumerable<ReactionReadDto> MapToResponse(this IEnumerable<Reaction> reactions) =>
        reactions.Select(r => r.MapToResponse());
}