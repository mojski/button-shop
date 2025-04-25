namespace ButtonShop.Application.Exceptions
{
    public abstract class NotFoundException : ApplicationException
    {
        protected NotFoundException(string message, Exception? innerException = null) : base(message, innerException)
        {
        }

        public override string Code => "Not found";
    }
}
