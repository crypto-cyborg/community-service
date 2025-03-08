using CommunityService.API.Exceptions;
using CommunityService.API.Extensions;
using CommunityService.Application.Models.Requests;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;
using MongoDB.Bson;

namespace CommunityService.API.Endpoints.Posts;

public class Comment(ICommunicationService communicationService) : Endpoint<CommentRequest, IResult>
{
    public override void Configure()
    {
        Post("/posts/{postId}/comments");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(CommentRequest req, CancellationToken ct)
    {
        var postId = Route<string>("postId");
        var userId = HttpContext.GetUserId();

        if (userId == Guid.Empty) return TypedResults.Unauthorized();

        var isValidObjectId = ObjectId.TryParse(postId, out _);

        if (!isValidObjectId) return TypedResults.Problem("Invalid post id", statusCode: 400);

        var result = await communicationService.CommentPost(postId!, userId, req.Text);

        return result.Match<IResult>(
            Succ: comment => TypedResults.Ok(comment),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}