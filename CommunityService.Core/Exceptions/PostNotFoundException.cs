namespace CommunityService.Core.Exceptions;

public class PostNotFoundException(string message = "Post does not exist") : Exception(message)
{
}