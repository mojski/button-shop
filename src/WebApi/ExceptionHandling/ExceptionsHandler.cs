using System.Net;
using ButtonShop.Application.Validation;
using ButtonShop.Domain.Buttons.Exceptions;

namespace ButtonShop.WebApi.ExceptionHandling;

public class ExceptionsHandler : IExceptionHandler
{
    private const string DEFAULT_TITLE = "An error occurred";

    private readonly IProblemDetailsService problemDetailsService;

    public ExceptionsHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException validationException => ((int)HttpStatusCode.BadRequest, validationException.Code),
            Application.ApplicationException applicationException => ((int)HttpStatusCode.BadRequest, applicationException.Code),
            DomainException domainException => ((int)HttpStatusCode.BadRequest, domainException.Code),
            _ => ((int)HttpStatusCode.InternalServerError, DEFAULT_TITLE),
        };

        httpContext.Response.StatusCode = statusCode;

        return await this.problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = title,
                Detail = exception.Message,
            },
            Exception = exception,
        });
    }
}
