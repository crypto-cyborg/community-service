using CommunityService.Application.Models.Dto;
using CommunityService.Core.Models;

namespace CommunityService.API.Endpoints.Posts;

public static class ReplyMapping
{
    public static ReplyReadDto MapToResponse(this Reply r) =>
        new(r.Id, r.UserId, r.CommentId, r.Text, r.Time);

    public static IEnumerable<ReplyReadDto> MapToResponse(this IEnumerable<Reply> replies) =>
        replies.Select(r => r.MapToResponse());
}