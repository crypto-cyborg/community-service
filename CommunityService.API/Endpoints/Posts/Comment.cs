using CommunityService.API.Exceptions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;
using MongoDB.Bson;

namespace CommunityService.API.Endpoints.Posts;

public record CommentRequest(Guid UserId, string Text);

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

        var isValidObjectId = ObjectId.TryParse(postId, out _);

        if (!isValidObjectId) return TypedResults.Problem("Invalid post id", statusCode: 400);

        var result = await communicationService.CommentPost(postId!, req.UserId, req.Text);

        return result.Match<IResult>(
            Succ: comment => TypedResults.Ok(comment),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}