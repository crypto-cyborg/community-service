namespace CommunityService.Application.Models.Requests;

public abstract record CreatePostRequest(string Topic, string Text, string[] Tags);

