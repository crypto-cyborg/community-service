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
    public async Task<Fin<IEnumerable<PostExtensions.PostReadDto>>> GetAllPosts()
    {
        var posts = fctx.Posts.AsNoTracking().AsEnumerable();

        return Fin<IEnumerable<PostExtensions.PostReadDto>>.Succ(posts.MapToResponse(isPreview: true));
    }

    public async Task<Fin<Post>> CreatePost(PostExtensions.CreatePostDto dto)
    {
        var user = await userService.Exists(dto.UserId.ToString());

        if (!user.Found)
        {
            return Fin<Post>.Fail(new UserNotFoundException($"User {dto.UserId} does not exist"));
        }

        var tags = await tagsService.EnsureCreated(dto.Tags);
        var post = PostExtensions.Create(new Guid(user.Id), dto.Topic, dto.Text, dto.Tags);

        await fctx.Posts.AddAsync(post);
        await fctx.SaveChangesAsync();

        await unitOfWork.SaveChangesAsync();

        return post;
    }
}