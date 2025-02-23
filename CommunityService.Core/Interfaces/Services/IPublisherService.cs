using CommunityService.Core.Models;
using LanguageExt;

namespace CommunityService.Core.Interfaces.Services;

public interface IPublisherService
{
    Task<User> GetPublisher(Guid id);
}