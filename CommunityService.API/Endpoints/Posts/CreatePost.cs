using CommunityService.API.Exceptions;
using CommunityService.API.Extensions;
using CommunityService.Application.Interfaces;
using CommunityService.Application.Models.Requests;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class CreatePost(IPostsService postsService, IPublisherService publisherService)
    : Endpoint<CreatePostRequest, IResult>
{
    public override void Configure()
    {
        Post("/posts");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(
        CreatePostRequest req,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();
        if (userId == Guid.Empty) return TypedResults.Unauthorized();
        
        var postResult = await postsService.CreatePost(userId, req);
        var publisher = await publisherService.GetPublisher(userId);

        return postResult.Match<IResult>(
            succ => Results.Ok(succ.MapToResponse(publisher, isPreview: true)),
            err => Results.BadRequest(err.ToProblem()));
    }
}