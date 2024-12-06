using CommunityService.Core.Interfaces;
using CommunityService.Core.Models;
using CommunityService.Persistence.Repositories;

namespace CommunityService.API.Extensions;

public static partial class ApiExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Post>, PostsRepository>();
    }
}