namespace ButtonShop.Infrastructure;

internal class InstanceIdentifier
{
    public Guid Id { get; }

    public InstanceIdentifier()
    {
        Id = Guid.NewGuid();
    }
}
