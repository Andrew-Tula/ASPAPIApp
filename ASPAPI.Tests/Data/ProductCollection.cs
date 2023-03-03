using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class ProductCollection
{
    public IQueryable<Product> Products { get; }

    public ProductCollection()
    {
        Products = new List<Product> {
            new Product{ Id = 1, Name = "Milk", Price = 11},
            new Product{ Id = 2, Name = "Meat", Price = 17 },
            new Product{ Id = 3, Name = "Eggs", Price = 3 },
            new Product{ Id = 4, Name = "Soup", Price = 5 },
        }.AsQueryable();
    }
}
