using System.Net;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace CommunityService.API.Exceptions;

public static class ExceptionConverter
{
    public static ProblemDetails ToProblem(this Error error, HttpStatusCode code = HttpStatusCode.InternalServerError)
    {
        return new ProblemDetails
        {
            Type = error.Exception.First().GetType().Name,
            Status = (int)code,
            Title = "An error occured",
            Detail = error.Message,
        };
    }
}