using CommunityService.Core.Extensions;
using LanguageExt;

namespace CommunityService.Core.Interfaces.Services;

public interface IPostsService
{
    Task<Fin<IEnumerable<PostExtensions.PostReadDto>>> GetAllPosts();
}