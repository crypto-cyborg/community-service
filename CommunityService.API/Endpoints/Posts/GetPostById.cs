using CommunityService.API.Mapping;
using CommunityService.Application.Interfaces;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class GetPostById(IPostsService postsService, IPublisherService publisherService, Mapper mapper)
    : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("/posts/{id}");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
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
                var res = mapper.PostsMapper.MapToResponse(post).Result;

                return TypedResults.Ok(res);
            },
            Fail: err => TypedResults.BadRequest(err));
    }
}