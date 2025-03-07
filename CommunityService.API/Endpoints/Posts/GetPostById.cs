using CommunityService.Application.Interfaces;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class GetPostById(IPostsService postsService, IPublisherService publisherService)
    : Endpoint<PostExtensions.PostReadDto, IResult>
{
    public override void Configure()
    {
        Get("/posts/{id}");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(PostExtensions.PostReadDto req, CancellationToken ct)
    {
        var postId = Route<string>("id");

        if (postId is null)
        {
            return TypedResults.BadRequest("Empty request");
        }

        var postResult = await postsService.GetPostById(postId);

        return postResult.Match<IResult>(
            Succ: post =>
            {
                var publisher = publisherService.GetPublisher(post.UserId).GetAwaiter().GetResult();
                var res = post.MapToResponse(publisher, isPreview: false);

                return TypedResults.Ok(res);
            },
            Fail: err => TypedResults.BadRequest(err));
    }
}