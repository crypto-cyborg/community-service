using CommunityService.Core.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CommunityService.Persistence.Contexts;

public class ForumContext(DbContextOptions opts) : DbContext(opts)
{
    public DbSet<Post> Posts { get; init; }
    public DbSet<Comment> Comments { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Post>().ToCollection("posts");
        modelBuilder.Entity<Comment>().ToCollection("comments");
    }
}