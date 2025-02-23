using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommunityService.Core.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required Guid UserId { get; set; }

    public IEnumerable<string> Tags { get; set; } = [];

    public DateTimeOffset Time { get; set; } = DateTimeOffset.Now;
    
    public required string Topic { get; set; }
    public required string? Text { get; set; }

    public List<Reaction> Reactions { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
}