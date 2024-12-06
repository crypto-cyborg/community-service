using CommunityService.Core.Models;

namespace CommunityService.Core.Extensions;

public static class PostExtensions
{
    public record PostReadDto(
        Guid Id,
        UserExtensions.UserReadDto User,
        IEnumerable<Tag> Tags,
        DateTimeOffset Time,
        string Topic,
        string Text,
        IEnumerable<ReactionExtensions.ReactionReadDto> Reactions);

    public static PostReadDto MapToResponse(this Post post)
        => new(post.Id, post.User.MapToResponse(), post.Tags, post.Time, post.Topic, post.Text,
            post.Reactions.MapToResponse());
}