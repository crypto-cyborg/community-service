using CommunityService.Application.Models.Dto;
using CommunityService.Core.Models;

namespace CommunityService.Application.Models.Responses;

public record GetPostByIdResponse(
    string Id,
    UserReadDto User,
    IEnumerable<string> Tags,
    DateTimeOffset Time,
    string Topic,
    string Text,
    IEnumerable<ReactionReadDto> Reactions,
    IEnumerable<CommentReadDto> Comments);