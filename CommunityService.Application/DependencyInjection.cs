using CommunityService.Application.Services;
using CommunityService.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityService.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPublisherService, PublisherService>();
        services.AddScoped<IPostsService, PostsService>();
        services.AddScoped<ITagsService, TagsService>();
    }
}