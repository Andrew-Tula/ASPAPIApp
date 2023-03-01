using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class OrderItemCollection
{
    public IQueryable<OrderItem> OrderItems { get; }

    public OrderItemCollection()
    {
        OrderItems = new List<OrderItem> {
            new OrderItem{ Id = 1, ProductCount = 8 },
            new OrderItem{ Id = 2, ProductCount = 25 },
            new OrderItem{ Id = 3, ProductCount = 1 },
            new OrderItem{ Id = 4, ProductCount = 101 },
        }.AsQueryable();
    }
}
