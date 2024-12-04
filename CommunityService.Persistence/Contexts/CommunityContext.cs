using CommunityService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Persistence.Contexts;

public class CommunityContext(DbContextOptions<CommunityContext> options) 
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ReactionType> ReactionTypes { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
}