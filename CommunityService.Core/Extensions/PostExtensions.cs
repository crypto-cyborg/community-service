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

    public sealed record CreatePostDto(Guid UserId, string Topic, string Text, string[] Tags);

    public static Post Create(Guid userId, string topic, string text, ICollection<Tag>? tags = null) =>
        new Post { UserId = userId, Topic = topic, Text = text, Tags = tags ?? [] };

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