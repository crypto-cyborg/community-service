using CommunityService.API.Exceptions;
using CommunityService.Application.Models.Requests;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;
using MongoDB.Bson;

namespace CommunityService.API.Endpoints.Reactions;

public class Undo(IReactionService reactionService) : Endpoint<UndoRequest, IResult>
{
    public override void Configure()
    {
        Delete("posts/{postId}/reactions");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(UndoRequest req, CancellationToken ct)
    {
        var postId = Route<string>("postId");
        var userId = HttpContext.Request.Headers["userId"].FirstOrDefault();

        var isValidObjectId = ObjectId.TryParse(postId, out _);

        if (!isValidObjectId) return TypedResults.Problem("Invalid post ID", statusCode: 400);
        if (userId is null) return TypedResults.Unauthorized();

        var result = await reactionService.Undo(postId!, new Guid(userId), req.ReactionTypeId);
        
        return result.Match<IResult>(
            Succ: reaction => TypedResults.Ok(reaction),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}