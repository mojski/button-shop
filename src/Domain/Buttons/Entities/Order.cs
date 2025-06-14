using ButtonShop.Domain.Exceptions;

namespace ButtonShop.Domain.Entities;

public sealed class Order
{
    public string CustomerName { get; private set; }
    public Guid Id { get; init; }
    public IReadOnlyDictionary<ButtonColors, int> Items => items;
    public Dictionary<ButtonColors, int> items { get; private set; }
    public string ShippingAddress { get; private set; }
    public OrderStatuses Status { get; private set; }

    public Order(Guid id, string customerName, string shippingAddress)
    {
        this.CustomerName = customerName;
        this.Id = id;
        this.ShippingAddress = shippingAddress;
        this.Status = OrderStatuses.Awaiting;
        this.items = new Dictionary<ButtonColors, int>();
    }

    public void AddItems(string color, int quantity)
    {
        if (Enum.TryParse(typeof(ButtonColors), color, ignoreCase: true, out var result) is false)
        {
            throw new ButtonColorNotSupportedException(color);
        }
        
        var parsedColor = (ButtonColors)result;

        if (this.Items.ContainsKey(parsedColor))
        {
            this.items[parsedColor] += quantity;
        }
        else
        {
            this.items[parsedColor] = quantity;
        }
    }

    public void AddItems(ButtonColors color, int quantity)
    {
        if (this.items.ContainsKey(color))
        {
            this.items[color] += quantity;
        }
        else
        {
            this.items[color] = quantity;
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
