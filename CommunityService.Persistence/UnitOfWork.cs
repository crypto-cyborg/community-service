using CommunityService.Core.Interfaces;
using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;
using CommunityService.Persistence.Repositories;

namespace CommunityService.Persistence;

public class UnitOfWork(
    CommunityContext context,
    PostsRepository postsRepository,
    UserRepository userRepository,
    TagsRepository tagsRepository) : IDisposable
{
    public int SaveChanges() => context.SaveChanges();
    
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

    public IRepository<Post> PostsRepository { get; } = postsRepository;
    public IRepository<User> UserRepository { get; } = userRepository;
    public IRepository<Tag> TagsRepository { get; } = tagsRepository;
    
    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}