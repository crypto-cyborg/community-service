using CommunityService.Application.Models.Requests;
using CommunityService.Core.Extensions;
using CommunityService.Core.Models;
using LanguageExt;

namespace CommunityService.Application.Interfaces;

public interface IPostsService
{
    Task<Fin<IEnumerable<Post>>> GetAllPosts();
    Task<Fin<Post>> GetPostById(string id);
    Task<Fin<Post>> CreatePost(Guid userId, CreatePostRequest dto);
    Task<Fin<Post>> Delete(string postId);
}