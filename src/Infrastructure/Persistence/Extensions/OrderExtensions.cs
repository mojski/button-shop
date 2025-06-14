using ButtonShop.Domain.Entities;
using ButtonShop.Infrastructure.Persistence.Models;

namespace ButtonShop.Infrastructure.Persistence.Extensions;

internal static class OrderExtensions
{
    public static OrderDbModel ToDbModel(this Order src)
    {
        return new OrderDbModel
        {
            Id = src.Id,
            CustomerName = src.CustomerName,
            ShippingAddress = src.ShippingAddress,
            Status = (int)src.Status,
            Items = src.Items.ToDictionary(item => (int)item.Key, item =>  item.Value),
        };
    }
}
