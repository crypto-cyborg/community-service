using CommunityService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Persistence.Contexts;

public class CommunityContext(DbContextOptions<CommunityContext> options) 
    : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Tag> Tags { get; init; }
    public DbSet<ReactionType> ReactionTypes { get; init; }
    public DbSet<Reaction> Reactions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Tag>().HasKey(t => t.Name);
    }
}