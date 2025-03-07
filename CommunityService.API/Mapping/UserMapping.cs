using CommunityService.Application.Models.Dto;
using CommunityService.Core.Models;

namespace CommunityService.API.Mapping;

public static class UserMapping
{
    public static UserReadDto MapToResponse(this User u) =>
        new UserReadDto(u.Id, u.Username, u.ImageUrl);
}