using CommunityService.Core.Exceptions;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Persistence;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class PostsService(UnitOfWork unitOfWork) : IPostsService
{
    public async Task<Fin<IEnumerable<PostExtensions.PostReadDto>>> GetAllPosts()
    {
        var posts = await (await unitOfWork.PostsRepository.GetAsync())
            .AsNoTracking()
            .ToListAsync();

        return Fin<IEnumerable<PostExtensions.PostReadDto>>.Succ(posts.MapToResponse(isPreview: true));
    }

    public async Task<Fin<Post>> CreatePost(PostExtensions.CreatePostDto dto)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(dto.UserId, includeProperties: "Posts");

        if (user is null) return new Fin.Fail<Post>(new UserNotFoundException());

        var post = PostExtensions.Create(dto.UserId, dto.Topic, dto.Text);

        await unitOfWork.PostsRepository.InsertAsync(post);
        user.Posts.Add(post);

        await unitOfWork.SaveChangesAsync();

        return post;
    }
}