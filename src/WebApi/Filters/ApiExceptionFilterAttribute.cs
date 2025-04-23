namespace ButtonShop.WebApi.Filters;

internal sealed class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is NotImplementedException)
        {
            HandleValidationException(context, (NotImplementedException)context.Exception);
        }
    }

    private static void HandleValidationException(ExceptionContext context, NotImplementedException exception)
    {
        var response = new
        {
            //exception.Code,
            Error = exception.Message,
            HiddenMessage = "kutas",
            //Response = exception.Errors,
        };

        context.Result = new ObjectResult(response)
        {
            ContentTypes =
            [
                MediaTypeNames.Application.Json,
            ],
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        context.ExceptionHandled = true;
    }
}
