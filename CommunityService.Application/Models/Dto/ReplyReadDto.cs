namespace CommunityService.Application.Models.Dto;

public record ReplyReadDto(
    string Id,
    Guid UserId,
    string CommentId,
    string Text,
    DateTime Time);