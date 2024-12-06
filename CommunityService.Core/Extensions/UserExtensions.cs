using CommunityService.Core.Models;

namespace CommunityService.Core.Extensions;

public static class UserExtensions
{
    public record UserReadDto(Guid UserId, string Username, string ImageUrl);

    public static UserReadDto MapToResponse(this User user) =>
        new(user.UserId, user.Username, user.ImageUrl);
}