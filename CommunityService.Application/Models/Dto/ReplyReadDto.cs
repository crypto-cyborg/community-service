namespace CommunityService.Application.Models.Dto;

public sealed record ReplyReadDto(
    string Id,
    Guid UserId,
    string CommentId,
    string Text,
    DateTime Time);