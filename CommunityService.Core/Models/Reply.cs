﻿using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommunityService.Core.Models;

public class Reply
{
    [BsonId]
    [Key]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required Guid UserId { get; set; }
    public required string CommentId { get; set; }
    public required string Text { get; set; }

    public DateTime Time { get; set; } = DateTime.UtcNow;

    public List<Reply> Replies { get; set; } = [];
}