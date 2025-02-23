using CommunityService.API.Exceptions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;
using MongoDB.Bson;

namespace CommunityService.API.Endpoints.Reactions;

public record UndoRequest(int ReactionTypeId);

public class Undo(IReactionService reactionService) : Endpoint<UndoRequest, IResult>
{
    public override void Configure()
    {
        Delete("posts/{postId}/reactions/{userId}");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(UndoRequest req, CancellationToken ct)
    {
        var postId = Route<string>("postId");
        var userId = Route<Guid>("userId");

        var isValidObjectId = ObjectId.TryParse(postId, out _);

        if (!isValidObjectId) return TypedResults.Problem("Invalid post ID", statusCode: 400);

        var result = await reactionService.Undo(postId!, userId, req.ReactionTypeId);
        
        return result.Match<IResult>(
            Succ: reaction => TypedResults.Ok(reaction),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}