using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;

namespace CommunityService.Persistence.Repositories;

public class ReactionRepository(CommunityContext context) : RepositoryBase<Reaction, CommunityContext>(context)
{
}