using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;

namespace CommunityService.Persistence.Repositories;

public class CommentsRepository(ForumContext context) : RepositoryBase<Comment, ForumContext>(context)
{
}