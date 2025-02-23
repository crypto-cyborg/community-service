using CommunityService.Persistence.Contexts;
using CommunityService.Persistence.Models;
using CommunityService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CommunityService.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CommunityContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("default-psql")));

        var mongoSettings = configuration.GetSection(MongoDbSettings.Settings).Get<MongoDbSettings>();
        if (mongoSettings is null) throw new MongoConfigurationException("Mongo configuration not found");
        
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.Settings));
        services.AddDbContext<ForumContext>(opts =>
            opts.UseMongoDB(mongoSettings.Uri, mongoSettings.DatabaseName));

        services.AddScoped<PostsRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<TagsRepository>();
        services.AddScoped<ReactionRepository>();
        services.AddScoped<CommentsRepository>();

        services.AddScoped<UnitOfWork>();
    }
}