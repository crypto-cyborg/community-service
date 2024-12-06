using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;

namespace CommunityService.Persistence.Repositories;

public class PostsRepository(CommunityContext context) : RepositoryBase<Post>(context) { }