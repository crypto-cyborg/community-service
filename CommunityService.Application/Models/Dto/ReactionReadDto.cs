using CommunityService.Application.Models.Responses;
using CommunityService.Core.Models;

namespace CommunityService.Application.Models.Dto;

public record ReactionReadDto(
    Guid Id,
    UserReadDto UserId,
    ReactionType Type,
    DateTimeOffset Time);