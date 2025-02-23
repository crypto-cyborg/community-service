using CommunityService.Infrastructure.ServiceClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<Users.UsersClient>(opts =>
            opts.Address = new Uri(configuration["gRPC:http"]!));

        services.AddScoped<UserService>();
    }
}