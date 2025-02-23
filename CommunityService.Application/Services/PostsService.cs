using CommunityService.Core.Exceptions;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.ServiceClients;
using CommunityService.Persistence;
using CommunityService.Persistence.Contexts;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class PostsService(UnitOfWork unitOfWork, ITagsService tagsService, ForumContext fctx, UserService userService)
    : IPostsService
{
    public Fin<IEnumerable<Post>> GetAllPosts()
    {
        var posts = fctx.Posts.AsNoTracking().AsEnumerable().ToList();

        // var userIds = posts.Select(p => p.UserId).Distinct().ToList();
        // var users = userIds.Select(id => unitOfWork.UserRepository.GetByIdAsync(id).GetAwaiter().GetResult());
        //
        // var userDict = users.ToDictionary(u => u.Id, u => u);

        foreach (var post in posts)
        {
            post.Comments = fctx.Comments.Where(c => c.PostId == post.Id).ToList();
        }

        return Fin<IEnumerable<Post>>.Succ(posts);
    }

    public async Task<Fin<Post>> GetPostById(string id)
    {
        var post = await fctx.Posts.FirstOrDefaultAsync(p => p.Id == id);

        return post ?? Fin<Post>.Fail(new PostNotFoundException());
    }

    public async Task<Fin<Post>> CreatePost(PostExtensions.CreatePostDto dto)
    {
        var user = await userService.Exists(dto.UserId.ToString());

        if (!user.Found)
        {
            return Fin<Post>.Fail(new UserNotFoundException($"User {dto.UserId} does not exist"));
        }

        var newUser = new User { Id = new Guid(user.Id), Username = user.Username };
        await unitOfWork.UserRepository.InsertAsync(newUser);

        var tags = await tagsService.EnsureCreated(dto.Tags);
        var post = PostExtensions.Create(newUser.Id, dto.Topic, dto.Text, dto.Tags);

        await fctx.Posts.AddAsync(post);
        await fctx.SaveChangesAsync();

        await unitOfWork.SaveChangesAsync();

        return post;
    }
}