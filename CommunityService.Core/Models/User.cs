namespace CommunityService.Core.Models;

public class User
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<Post> Posts { get; set; } = [];
}