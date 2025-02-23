using CommunityService.Core.Extensions;
using CommunityService.Core.Models;
using LanguageExt;

namespace CommunityService.Core.Interfaces.Services;

public interface IPostsService
{
    Task<Fin<IEnumerable<Post>>> GetAllPosts();
    Task<Fin<Post>> GetPostById(string id);
    Task<Fin<Post>> CreatePost(PostExtensions.CreatePostDto dto);
}