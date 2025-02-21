using CommunityService.Core.Models;

namespace CommunityService.Core.Interfaces.Services;

public interface ITagsService
{
    Task<IEnumerable<Tag>> GetAllTags();
    Task<List<Tag>> EnsureCreated(string[] tagNames);
}