using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class PostsService(IRepository<Post> postsRepository) : IPostsService
{
    public async Task<Fin<IEnumerable<PostExtensions.PostReadDto>>> GetAllPosts()
    {
        var posts = await (await postsRepository.GetAsync()).AsNoTracking().ToListAsync();
        
        return Fin<IEnumerable<PostExtensions.PostReadDto>>.Succ(posts.MapToResponse(true));
    }
}