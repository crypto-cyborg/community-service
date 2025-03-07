using CommunityService.API.Mapping;

namespace CommunityService.API.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddScoped<CommentMapping>();
        services.AddScoped<PostsMapping>();

        services.AddScoped<Mapper>();
        
        return services;
    }
}