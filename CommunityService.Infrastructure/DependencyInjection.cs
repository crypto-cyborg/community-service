using CommunityService.Infrastructure.RabbitMq;
using CommunityService.Infrastructure.ServiceClients;
using CommunityService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CommunityService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<Users.UsersClient>(opts =>
            opts.Address = new Uri(configuration["GrpcOptions:HttpHost"]!));

        services.AddSingleton(new ConnectionFactory { HostName = configuration["RabbitMq:Host"]! });
        services.AddSingleton<Producer>();
        services.AddSingleton<Consumer>();
        services.AddHostedService<ReactionBackgroundService>();

        services.AddScoped<UserService>();
    }
}