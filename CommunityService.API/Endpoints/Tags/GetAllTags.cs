using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.Tags;

public class GetAllTags(ITagsService tagsService) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("/tags");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        var tags = await tagsService.GetAllTags();

        return TypedResults.Ok(tags);
    }
}