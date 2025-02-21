using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Persistence.Repositories;

public class TagsRepository(CommunityContext context): RepositoryBase<Tag>(context)
{
   public override async Task<Tag?> GetByIdAsync(object id, string includeProperties = "")
   {
      return await DbSet.FirstOrDefaultAsync(t => t.Name == id.ToString());
   }
}