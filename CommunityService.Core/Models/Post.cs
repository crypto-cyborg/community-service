namespace CommunityService.Core.Models;

public class Post
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public ICollection<Tag> Tags { get; set; } = [];

    public DateTimeOffset Time { get; set; } = DateTimeOffset.Now;
    
    public required string Topic { get; set; }
    public required string? Text { get; set; }

    public ICollection<Reaction> Reactions { get; set; } = [];
}