namespace CommunityService.Persistence.Models;

public class MongoDbSettings
{
    public static string Settings = "MongoDbSettings";
    
    public required string Uri { get; set; }
    public required string DatabaseName { get; set; }
}