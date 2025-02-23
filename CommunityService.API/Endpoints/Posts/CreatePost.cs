using CommunityService.API.Exceptions;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class CreatePost(IPostsService postsService, IPublisherService publisherService)
    : Endpoint<PostExtensions.CreatePostDto, IResult>
{
    public override void Configure()
    {
        Post("/posts");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(
        PostExtensions.CreatePostDto req,
        CancellationToken ct)
    {
        var postResult = await postsService.CreatePost(req);
        var publisher = await publisherService.GetPublisher(req.UserId);

        return postResult.Match<IResult>(
            succ => Results.Ok(succ.MapToResponse(publisher, isPreview: true)),
            err => Results.BadRequest(err.ToProblem()));
    }
}