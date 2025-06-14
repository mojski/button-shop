using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.Persistence.Models;

namespace ButtonShop.Infrastructure.Persistence.Extensions;

internal static class OrderDbModelExtensions
{
    public static Order ToDomainModel(this OrderDbModel src)
    {
        var dest = new Order(src.Id, src.CustomerName, src.ShippingAddress);

        foreach (var item in src.Items)
        {
            var color = (ButtonColors)item.Key;

            dest.AddItems(color, item.Value);
        }

        var status = (OrderStatuses)src.Status;

        if (status == OrderStatuses.Shipped)
        {
            dest.Ship();
        }

        return dest;
    }
}
