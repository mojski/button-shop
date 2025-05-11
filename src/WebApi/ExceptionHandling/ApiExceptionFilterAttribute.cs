using System.Runtime.CompilerServices;
using ButtonShop.Application.Exceptions;
using ButtonShop.Application.Validation;

namespace ButtonShop.WebApi.ExceptionHandling;

internal sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{   
    private readonly ILogger<ApiExceptionFilterAttribute> logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        this.logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case NotFoundException notFoundException:
                HandleNotFoundException(context, notFoundException);
                this.logger.LogInformation("Resource not found for Id:{Id}, with message:{Message}", notFoundException.Id, notFoundException.Message);
                break;

            case ValidationException validationException:
                HandleValidationException(context, validationException);
                this.logger.LogInformation("Validation errors occured:{Message}", validationException.Message);

                break;

            default:
                this.logger.LogError(context.Exception, "{Message}", context.Exception.Message);
                HandleUnknownException(context);

                break;
        }
        base.OnException(context);
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        context.ExceptionHandled = false;
    }

    private static void HandleValidationException(ExceptionContext context, ValidationException exception)
    {
        var response = new
        {
            exception.Code,
            Error = exception.Message,
            Response = exception.Errors,
        };

        context.Result = GetResult(response, StatusCodes.Status400BadRequest);
        context.ExceptionHandled = true;
    }

    private static void HandleNotFoundException(ExceptionContext context, NotFoundException exception)
    {
        var response = new
        {
            exception.Code,
            Error = exception.Message,
        };

        context.Result = GetResult(response, StatusCodes.Status404NotFound);
        context.ExceptionHandled = true;
    }

    private static ObjectResult GetResult(object response, int statusCode)
    {
        return new ObjectResult(response)
        {
            ContentTypes =
            [
                MediaTypeNames.Application.Json,
            ],
            StatusCode = statusCode,
        };
    }
}
