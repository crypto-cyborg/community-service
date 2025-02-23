using CommunityService.Core.Models;
using LanguageExt;

namespace CommunityService.Core.Interfaces.Services;

public interface ICommunicationService
{
    Task<Fin<Comment>> CommentPost(string postId, Guid userId, string text);
    Task<Fin<Reply>> ReplyToComment(string commentId, Guid userId, string text);
}