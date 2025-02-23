using CommunityService.API.Exceptions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;
using MongoDB.Bson;

namespace CommunityService.API.Endpoints.Comments;

public record ReplyRequest(Guid UserId, string Text);

public class Reply(ICommunicationService communicationService) : Endpoint<ReplyRequest, IResult>
{
    public override void Configure()
    {
        Post("posts/comments/{commentId}");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(ReplyRequest req, CancellationToken ct)
    {
        var commentId = Route<string>("commentId");

        var isValidObjectId = ObjectId.TryParse(commentId, out _);

        if (!isValidObjectId) return TypedResults.Problem("Invalid comment id", statusCode: 400);

        var result = await communicationService.ReplyToComment(commentId!, req.UserId, req.Text);

        return result.Match<IResult>(
            Succ: reply => TypedResults.Ok(reply),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}