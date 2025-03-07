using CommunityService.Application.Interfaces;
using CommunityService.Application.Models.Requests;
using CommunityService.Core.Exceptions;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.ServiceClients;
using CommunityService.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class PostsService(UnitOfWork unitOfWork, ITagsService tagsService, UserService userService)
    : IPostsService
{
    public async Task<Fin<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await unitOfWork.PostsRepository.GetAsync()
            .AsNoTracking()
            .ToListAsync();

        foreach (var post in posts)
        {
            post.Username = await GetUsername(post.UserId);
        }

        return Fin<IEnumerable<Post>>.Succ(posts);
    }

    public async Task<Fin<Post>> GetPostById(string id)
    {
        var post = await unitOfWork.PostsRepository.GetByIdAsync(id);

        if (post is null) return Fin<Post>.Fail(new PostNotFoundException());

        post.Username = await GetUsername(post.UserId);
        post.Reactions = await unitOfWork.ReactionRepository.GetAsync(r => r.PostId == post.Id)
            .AsNoTracking()
            .ToListAsync();
        post.Comments = await unitOfWork.CommentsRepository.GetAsync(c => c.PostId == post.Id)
            .AsNoTracking()
            .ToListAsync();
        post.LikesCount = post.Reactions.Count(r => r.TypeId == 1);

        return post;
    }

    public async Task<Fin<Post>> CreatePost(Guid userId, CreatePostRequest dto)
    {
        var userResponse = await userService.Exists(userId.ToString());

        if (!userResponse.Found)
        {
            return Fin<Post>.Fail(new UserNotFoundException($"User {userId} does not exist"));
        }

        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user is null)
        {
            user = new User { Id = new Guid(userResponse.Id), Username = userResponse.Username };
            await unitOfWork.UserRepository.InsertAsync(user);
        }

        var tags = await tagsService.EnsureCreated(dto.Tags);
        var post = PostExtensions.Create(user.Id, dto.Topic, dto.Text, dto.Tags);

        await unitOfWork.PostsRepository.InsertAsync(post);

        await unitOfWork.SaveCommunityChangesAsync();
        await unitOfWork.SaveForumChangesAsync();

        return post;
    }

    public async Task<Fin<Post>> Delete(string postId)
    {
        var post = await unitOfWork.PostsRepository.GetByIdAsync(postId);

        if (post is null) return Fin<Post>.Fail(new PostNotFoundException());

        unitOfWork.PostsRepository.Delete(post);

        await unitOfWork.SaveForumChangesAsync();

        return post;
    }

    private async Task<string> GetUsername(Guid userId)
    {
        var user = await unitOfWork.UserRepository.GetAsync(u => u.Id == userId)
            .FirstOrDefaultAsync();

        return user is null ? "Deleted user" : user.Username;
    }
}