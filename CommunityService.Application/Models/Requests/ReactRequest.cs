namespace CommunityService.Application.Models.Requests;

public sealed record ReactRequest(int ReactionTypeId);
public sealed record UndoRequest(int ReactionTypeId);

