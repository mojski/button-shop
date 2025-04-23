namespace ButtonShop.Application
{
    public abstract class ApplicationException : Exception
    {
        public abstract string Code { get; }
        public Guid Id { get; protected init; } = Guid.Empty;

        protected ApplicationException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
