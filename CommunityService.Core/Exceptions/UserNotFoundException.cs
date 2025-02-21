namespace CommunityService.Core.Exceptions;

public class UserNotFoundException(string message = "User does not exist") : Exception(message)
{
    public int Code { get; } = 404;
}