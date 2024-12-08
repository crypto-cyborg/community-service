using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;

namespace CommunityService.Persistence.Repositories;

public class UserRepository(CommunityContext context) : RepositoryBase<User>(context) { }