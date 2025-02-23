using CommunityService.Core.Interfaces;
using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;
using CommunityService.Persistence.Repositories;

namespace CommunityService.Persistence;

public class UnitOfWork(
    CommunityContext communityContext,
    ForumContext forumContext,
    PostsRepository postsRepository,
    UserRepository userRepository,
    TagsRepository tagsRepository,
    ReactionRepository reactionRepository,
    CommentsRepository commentsRepository) : IDisposable
{
    public Task<int> SaveCommunityChangesAsync() => communityContext.SaveChangesAsync();
    public Task<int> SaveForumChangesAsync() => forumContext.SaveChangesAsync();

    public IRepository<Post> PostsRepository { get; } = postsRepository;
    public IRepository<Comment> CommentsRepository { get; } = commentsRepository;
    
    public IRepository<User> UserRepository { get; } = userRepository;
    public IRepository<Tag> TagsRepository { get; } = tagsRepository;
    public IRepository<Reaction> ReactionRepository { get; } = reactionRepository;
    
    public void Dispose()
    {
        communityContext.Dispose();
        forumContext.Dispose();
        GC.SuppressFinalize(this);
    }
}