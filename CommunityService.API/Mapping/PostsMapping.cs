using CommunityService.Application.Models.Responses;
using CommunityService.Core.Models;
using CommunityService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.API.Mapping;

public class PostsMapping(UnitOfWork unitOfWork, CommentMapping commentMapper)
{
    public async Task<GetPostByIdResponse> MapToResponse(Post p)
    {
        var reactions = await unitOfWork.ReactionRepository
            .GetAsync(r => r.PostId == p.Id)
            .Include(r => r.Type)
            .Include(r => r.User)
            .AsNoTracking()
            .ToListAsync();

        var comments = await unitOfWork.CommentsRepository
            .GetAsync(c => c.PostId == p.Id)
            .ToListAsync();

        var user = await unitOfWork.UserRepository
                   .GetByIdAsync(p.UserId) ??
                   new User { Id = Guid.Empty, Username = "Deleted user", ImageUrl = "" };

        return new GetPostByIdResponse(
            p.Id,
            user.MapToResponse(),
            p.Tags,
            p.Time,
            p.Topic,
            p.Text,
            reactions.MapToResponse(),
            await commentMapper.MapToResponse(comments));
    }
}