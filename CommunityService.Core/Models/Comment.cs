using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommunityService.Core.Models;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [Key]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required string PostId { get; set; }
    public required Guid UserId { get; set; }
    public required string Text { get; set; }
    
    public DateTime Time { get; set; } = DateTime.UtcNow;

    public List<Reply> Replies { get; set; } = [];
}