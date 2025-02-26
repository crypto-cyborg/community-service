using CommunityService.Core.Exceptions;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Persistence;
using CommunityService.Persistence.Contexts;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class CommunicationService(UnitOfWork unitOfWork) : ICommunicationService
{
    public async Task<Fin<Comment>> CommentPost(string postId, Guid userId, string text)
    {
        var comment = new Comment
        {
            PostId = postId,
            UserId = userId,
            Text = text
        };

        var post = await unitOfWork.PostsRepository.GetByIdAsync(postId);
        
        if (post is null) return Fin<Comment>.Fail(new PostNotFoundException());
        
        await unitOfWork.CommentsRepository.InsertAsync(comment);
        post.CommentsCount += 1;
        await unitOfWork.SaveForumChangesAsync();

        return comment;
    }

    public async Task<Fin<Reply>> ReplyToComment(string commentId, Guid userId, string text)
    {
        var reply = new Reply
        {
            CommentId = commentId,
            UserId = userId,
            Text = text
        };

        var comment = await unitOfWork.CommentsRepository.GetByIdAsync(commentId);

        if (comment is null) return Fin<Reply>.Fail(new InvalidOperationException("Cannot reply"));
        
        // TODO: Too many database requests
        var post = await unitOfWork.PostsRepository.GetAsync(p => p.Id == comment.PostId).FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("Cannot find required post");
        post.CommentsCount -= 1;
        
        comment.Replies.Add(reply);
        await unitOfWork.SaveForumChangesAsync();

        return reply;
    }
}