using System.Net;
using CodeWright.Common.Asp;
using CodeWright.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CodeWright.Metadata.API.Controllers;

/// <summary>
/// Controller for displaying errors
/// </summary>
/// <remarks>
/// Based on : https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling
/// </remarks>
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    /// <summary>
    /// Handle errors
    /// </summary>
    /// <returns>The error response</returns>
    [Route("error")]
    public ErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (context == null)
            throw new InvalidInternalStateException(nameof(context));

        var exception = context.Error;
        HttpStatusCode code = HttpStatusCode.InternalServerError;

        if (exception is NotFoundException) code = HttpStatusCode.NotFound;
        else if (exception is BadRequestException) code = HttpStatusCode.BadRequest;

        Response.StatusCode = (int)code;

        return new ErrorResponse(exception); // Your error model
    }
}
