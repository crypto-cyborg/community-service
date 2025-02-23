using CommunityService.Core.Exceptions;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class CommunicationService(ForumContext ctx) : ICommunicationService
{
    public async Task<Fin<Comment>> CommentPost(string postId, Guid userId, string text)
    {
        var comment = new Comment
        {
            PostId = postId,
            UserId = userId,
            Text = text
        };

        var post = await ctx.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        
        if (post is null) return Fin<Comment>.Fail(new PostNotFoundException());

        ctx.Comments.Add(comment);
        post.Comments.Add(comment);
        await ctx.SaveChangesAsync();

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

        var comment = await ctx.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment is null) return Fin<Reply>.Fail(new InvalidOperationException("Cannot reply"));
        
        comment.Replies.Add(reply);
        await ctx.SaveChangesAsync();

        return reply;
    }
}