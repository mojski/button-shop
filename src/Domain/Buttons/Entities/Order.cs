using ButtonShop.Domain.Exceptions;

namespace ButtonShop.Domain.Entities;

public sealed class Order
{
    public string CustomerName { get; private set; }
    public Guid Id { get; set; }
    public Dictionary<ButtonColors, int> Items { get; private set; }
    public string ShippingAddress { get; private set; }
    public OrderStatuses Status { get; private set; }

    public Order(Guid id, string customerName, string shippingAddress)
    {
        this.CustomerName = customerName;
        this.Id = id;
        this.ShippingAddress = shippingAddress;
        this.Status = OrderStatuses.Awaiting;
        this.Items = new Dictionary<ButtonColors, int>();
    }

    public void AddItems(string color, int quantity)
    {
        if (Enum.TryParse(typeof(ButtonColors), color, ignoreCase: true, out var result) is false)
        {
            throw new ButtonColorNotSupportedException(color);
            // TODO add exception handling to request pipeline
        }

        var parsedColor = (ButtonColors)result;

        if (this.Items.TryAdd(parsedColor, quantity))
        {
            this.Items[parsedColor] += quantity;
        }
        else
        {
            this.Items[parsedColor] = quantity;
        }
    }

    public void Ship()
    {
        this.Status = OrderStatuses.Shipped;
    }
}

public enum ButtonColors
{
    Red,
    Green,
    Blue,
}

public enum OrderStatuses
{
    Awaiting,
    Shipped,
}
