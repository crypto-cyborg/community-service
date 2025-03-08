using CommunityService.API.Mapping;
using CommunityService.Application.Interfaces;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Posts;

public class GetPostById(IPostsService postsService, Mapper mapper)
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

        return await postResult.Match<Task<IResult>>(
            Succ: async post =>
            {
                var res = await mapper.PostsMapper.MapToResponse(post);

                return TypedResults.Ok(res);
            },
            Fail: err => Task.FromResult<IResult>(TypedResults.BadRequest(err)));
    }
}