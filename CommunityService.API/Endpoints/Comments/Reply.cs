using CommunityService.API.Exceptions;
using CommunityService.API.Extensions;
using CommunityService.Application.Models.Requests;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;
using MongoDB.Bson;

namespace CommunityService.API.Endpoints.Comments;

public class Reply(ICommunicationService communicationService) : Endpoint<ReplyRequest, IResult>
{
    public override void Configure()
    {
        Post("posts/comments/{commentId}");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(ReplyRequest req, CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();
        if (userId == Guid.Empty) return TypedResults.Unauthorized();
        
        var commentId = Route<string>("commentId");
        var isValidObjectId = ObjectId.TryParse(commentId, out _);
        if (!isValidObjectId) return TypedResults.Problem("Invalid comment id", statusCode: 400);

        var result = await communicationService.ReplyToComment(commentId!, userId, req.Text);

        return result.Match<IResult>(
            Succ: reply => TypedResults.Ok(reply),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}