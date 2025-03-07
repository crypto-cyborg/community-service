namespace CommunityService.Application.Models.Requests;

public abstract record ReactRequest(int ReactionTypeId);
public abstract record UndoRequest(int ReactionTypeId);

