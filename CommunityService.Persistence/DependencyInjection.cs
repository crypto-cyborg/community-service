using CommunityService.Persistence.Contexts;
using CommunityService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityService.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<CommunityContext>(opts =>
            opts.UseInMemoryDatabase("CommunityInMemo"));
        
        services.AddScoped<PostsRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<TagsRepository>();

        services.AddScoped<UnitOfWork>();
    }
}