using CommunityService.API.Exceptions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class GetAllPosts(IPostsService postsService) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("posts");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        var result = await postsService.GetAllPosts();

        return result.Match<IResult>(
            success => TypedResults.Ok(success),
            error => TypedResults.Problem(error.ToProblem())
        );
    }
}