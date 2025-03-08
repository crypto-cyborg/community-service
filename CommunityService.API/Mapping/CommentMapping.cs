using CommunityService.API.Endpoints.Posts;
using CommunityService.Application.Models.Dto;
using CommunityService.Core.Models;
using CommunityService.Persistence;
using Comment = CommunityService.Core.Models.Comment;

namespace CommunityService.API.Mapping;

public class CommentMapping(UnitOfWork unitOfWork)
{
    public async Task<CommentReadDto> MapToResponse(Comment c)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(c.UserId) ??
                   new User { Id = Guid.Empty, Username = "Deleted user", ImageUrl = "" };

        return new CommentReadDto(c.Id, c.PostId, user.MapToResponse(), c.Text, c.Time, c.Replies.MapToResponse());
    }

    public async Task<IEnumerable<CommentReadDto>> MapToResponse(IEnumerable<Comment> comments) =>
        await Task.WhenAll(comments.Select(MapToResponse));
}