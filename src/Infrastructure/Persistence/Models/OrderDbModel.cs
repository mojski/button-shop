namespace ButtonShop.Infrastructure.Persistence.Models;

public class OrderDbModel
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Dictionary<int, int> Items { get; set; } = new Dictionary<int, int>();
    public string ShippingAddress { get; set; } = string.Empty;
    public int Status { get; set; }
}
