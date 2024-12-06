using CommunityService.Application.Services;
using CommunityService.Core.Interfaces.Services;

namespace CommunityService.API.Extensions;

public partial class ApiExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPostsService, PostsService>();
    }
}