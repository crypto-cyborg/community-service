namespace CommunityService.Application.Models.Requests;

public sealed record CreatePostRequest(string Topic, string Text, string[] Tags);

