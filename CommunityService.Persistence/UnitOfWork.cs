using CommunityService.Persistence.Contexts;
using CommunityService.Persistence.Repositories;

namespace CommunityService.Persistence;

public class UnitOfWork(
    CommunityContext context,
    PostsRepository postsRepository,
    UserRepository userRepository) : IDisposable
{
    public int SaveChanges() => context.SaveChanges();
    
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

    public PostsRepository PostsRepository { get; init; } = postsRepository;
    public UserRepository UserRepository { get; init; } = userRepository;
    
    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}