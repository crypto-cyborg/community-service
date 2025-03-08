namespace CommunityService.Application.Models.Dto;

public sealed record CommentReadDto(
    string Id,
    string PostId,
    UserReadDto User,
    string Text,
    DateTime Time,
    IEnumerable<ReplyReadDto> Replies);