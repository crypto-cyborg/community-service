using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;

namespace CommunityService.Persistence.Repositories;

public class ReactionTypesRepository(CommunityContext context) : RepositoryBase<ReactionType, CommunityContext>(context)
{
}