using CommunityService.API.Exceptions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;
using MongoDB.Bson;

namespace CommunityService.API.Endpoints.Reactions;

public record ReactRequest(int ReactionTypeId);

public class React(IReactionService reactionService) : Endpoint<ReactRequest, IResult>
{
    public override void Configure()
    {
        Post("posts/{postId}/reactions/{userId}");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(ReactRequest req, CancellationToken ct)
    {
        var postId = Route<string>("postId");
        var userId = Route<Guid>("userId");

        var isValidObjectId = ObjectId.TryParse(postId, out _);

        if (!isValidObjectId) return TypedResults.Problem("Invalid post ID", statusCode: 400);

        var result = await reactionService.React(postId!, userId, req.ReactionTypeId);

        return result.Match<IResult>(
            Succ: reaction => TypedResults.Ok(reaction),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}