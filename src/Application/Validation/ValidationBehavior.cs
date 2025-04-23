
using System.Data;

namespace ButtonShop.Application.Validation
{
    internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> requestValidators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> requestValidators) =>
        this.requestValidators = requestValidators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (this.requestValidators.Any())
            {
                var fluentValidationContext = new ValidationContext<TRequest>(request);
                
                var validationResults = this.requestValidators.Select(v => v.ValidateAsync(fluentValidationContext, cancellationToken));

                await EvaluateResults(validationResults);
            }

            return await next();
        }

        private static async Task EvaluateResults(IEnumerable<Task<ValidationResult>> validation)
        {
            var validationResults = await Task.WhenAll(validation);

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(result => result is not null)
                .ToList();

            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }
        }
    }
}
