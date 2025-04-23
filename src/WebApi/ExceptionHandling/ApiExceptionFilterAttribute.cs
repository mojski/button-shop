using ButtonShop.Application.Validation;

namespace ButtonShop.WebApi.ExceptionHandling;

internal sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        => this.logger = logger;

    public override void OnException(ExceptionContext context)
    {
        this.logger.LogError(context.Exception, "{Message}", context.Exception.Message);

        switch (context.Exception)
        {
            case ValidationException validationException:
                HandleValidationException(context, validationException);

                break;
            default:
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

        context.Result = new ObjectResult(response)
        {
            ContentTypes =
            [
                MediaTypeNames.Application.Json,
            ],
            StatusCode = StatusCodes.Status400BadRequest,
        };

        context.ExceptionHandled = true;
    }
}
