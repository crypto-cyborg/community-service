using CommunityService.API.Exceptions;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class CreatePost(IPostsService postsService)
    : Endpoint<PostExtensions.CreatePostDto, IResult>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("posts");

        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(
        PostExtensions.CreatePostDto req,
        CancellationToken ct)
    {
        var result = await postsService.CreatePost(req);

        return result.Match(
            succ => Results.Ok(succ.MapToResponse(isPreview: true)),
            err => Results.BadRequest(err.ToProblem()));
    }
}