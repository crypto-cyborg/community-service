using CommunityService.Application.Models.Responses;
using CommunityService.Core.Models;
using CommunityService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.API.Mapping;

public class PostsMapping(UnitOfWork unitOfWork, CommentMapping commentMapper)
{
    public async Task<GetPostByIdResponse> MapToResponse(Post p)
    {
        var reactions = unitOfWork.ReactionRepository.GetAsync()
            .Include(r => r.Type)
            .AsNoTracking()
            .ToListAsync();
        var comments = unitOfWork.CommentsRepository.GetAsync(c => c.PostId == p.Id)
            .ToListAsync();
        var user = await unitOfWork.UserRepository.GetByIdAsync(p.UserId) ??
                   new User { Id = Guid.Empty, Username = "Deleted user", ImageUrl = "" };

        return new GetPostByIdResponse(
            p.Id,
            user.MapToResponse(),
            p.Tags,
            p.Time,
            p.Topic,
            p.Text!,
            reactions.Result.MapToResponse(),
            await commentMapper.MapToResponse(comments.Result));
    }
}