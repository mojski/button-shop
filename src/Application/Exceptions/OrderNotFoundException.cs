namespace ButtonShop.Application.Exceptions
{
    internal sealed class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id)
        : base($"Order with id {id}not found")
        {
            this.Id = id;
        }
    }
}
