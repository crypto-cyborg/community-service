using CommunityService.API.Exceptions;
using CommunityService.Application.Interfaces;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class DeletePost(IPostsService postsService) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Delete("/posts/{id}");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        var postId = Route<string>("id");

        if (postId is null) return TypedResults.BadRequest("Invalid post ID");

        var result = await postsService.Delete(postId);

        return result.Match<IResult>(
            Succ: post => TypedResults.Ok(post),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}