using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommunityService.Core.Models;

public class Reply
{
    [BsonId]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required string UserId { get; set; }
    public required string PostId { get; set; }
    public required string Text { get; set; }

    public DateTime Time { get; set; } = DateTime.UtcNow;

    public List<Reply> Replies { get; set; } = [];
}