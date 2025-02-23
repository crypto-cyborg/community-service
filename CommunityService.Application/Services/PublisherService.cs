using CommunityService.Core.Exceptions;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Persistence;
using LanguageExt;

namespace CommunityService.Application.Services;

public class PublisherService(UnitOfWork unitOfWork) : IPublisherService
{
    public async Task<User> GetPublisher(Guid id)
    {
        var publisher = await unitOfWork.UserRepository.GetByIdAsync(id);

        return publisher ?? new User { Id = new Guid(), Username = "Deleted User", ImageUrl = "" };
    }
}