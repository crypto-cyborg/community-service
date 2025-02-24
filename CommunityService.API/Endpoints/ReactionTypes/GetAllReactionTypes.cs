using CommunityService.API.Exceptions;
using CommunityService.Core.Interfaces.Services;
using FastEndpoints;

namespace CommunityService.API.Endpoints.ReactionTypes;

public class GetAllReactionTypes(IReactionService reactionService) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("/reactions/types");
        AllowAnonymous();
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        var result = await reactionService.GetAvailableTypes();

        return result.Match<IResult>(
            Succ: types => TypedResults.Ok(types),
            Fail: err => TypedResults.Problem(err.ToProblem()));
    }
}