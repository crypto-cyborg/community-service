namespace CommunityService.API.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var stringId = context.Request.Headers["userId"].FirstOrDefault();

        return stringId is null ? Guid.Empty : new Guid(stringId);
    }
}