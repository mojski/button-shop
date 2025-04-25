using System.Net;
using ButtonShop.Application.Exceptions;
using ButtonShop.Application.Validation;
using ButtonShop.Domain.Buttons.Exceptions;

namespace ButtonShop.WebApi.ExceptionHandling;

internal sealed class ExceptionsHandler : IExceptionHandler
{
    private const string DEFAULT_TITLE = "An error occurred";

    private readonly IProblemDetailsService problemDetailsService;
    private readonly IWebHostEnvironment environment;

    public ExceptionsHandler(IProblemDetailsService problemDetailsService, IWebHostEnvironment environment)
    {
        this.problemDetailsService = problemDetailsService;
        this.environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Invoked only when ApiExceptionFilter leaves the exception unhandled.
        var (statusCode, title) = exception switch
        {
            ValidationException validationException => ((int)HttpStatusCode.BadRequest, validationException.Code),
            NotFoundException notFoundException => ((int)HttpStatusCode.NotFound, notFoundException.Code),
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
                Detail = environment.IsDevelopment() ? exception.Message : null,
            },
            Exception = exception,
        });
    }
}
