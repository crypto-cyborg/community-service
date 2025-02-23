using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class TagsService(UnitOfWork unitOfWork) : ITagsService
{
    public async Task<IEnumerable<Tag>> GetAllTags() =>
        await unitOfWork.TagsRepository.GetAsync().AsNoTracking().ToListAsync();

    public async Task<List<Tag>> EnsureCreated(string[] tagNames)
    {
        List<Tag> tags = [];

        foreach (var name in tagNames)
        {
            var tag = await unitOfWork.TagsRepository.GetByIdAsync(name);

            if (tag is null)
            {
                tag = new Tag { Name = name };
                await unitOfWork.TagsRepository.InsertAsync(tag);
            }

            tags.Add(tag);
        }

        await unitOfWork.SaveCommunityChangesAsync();
        return tags;
    }
}