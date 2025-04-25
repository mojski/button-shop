using System.Diagnostics;

namespace ButtonShop.Application.Behaviors
{
    internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            this.logger.LogInformation("Start handling {RequestName}", requestName);
            try
            {
                var response = await next();

                this.logger.LogInformation("Finished {RequestName}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "{RequestName} failed", requestName);
                throw;
            }
        }
    }
}
