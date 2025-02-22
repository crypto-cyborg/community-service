namespace CommunityService.Core.Models;

public class Reaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required Guid UserId { get; set; }
    public virtual User User { get; set; }

    public required string PostId { get; set; }

    public required int TypeId { get; set; }
    public virtual ReactionType Type { get; set; }

    public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;
}