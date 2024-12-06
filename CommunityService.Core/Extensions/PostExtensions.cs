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
        string? Text,
        IEnumerable<ReactionExtensions.ReactionReadDto>? Reactions);

    public static PostReadDto MapToResponse(this Post post, bool isPreview)
    {
        var text = isPreview ? null : post.Text;
        var reactions = isPreview ? null : post.Reactions;

        return new PostReadDto(post.Id, post.User.MapToResponse(), post.Tags, post.Time, post.Topic, text,
            reactions?.MapToResponse());
    }

    public static IEnumerable<PostReadDto> MapToResponse(this IEnumerable<Post> posts, bool isPreview) =>
        posts.Select(p => p.MapToResponse(isPreview));
}