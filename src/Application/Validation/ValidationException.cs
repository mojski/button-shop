namespace ButtonShop.Application.Validation
{
    public sealed class ValidationException : ApplicationException
    {
        public ValidationException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        : this("Validation errors occured")
        {
            var errors = failures
                .GroupBy(e => e.ErrorCode, e => e.ErrorMessage)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray()
                );

            this.Errors = errors;
        }

        public override string Code => ValidationConstants.VALIDATION_ERROR_CODE;
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
    }
}
