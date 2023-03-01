using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class OrderCollection
{
    public IQueryable<Order> Orders { get; }

    public OrderCollection()
    {
        Orders = new List<Order> {
            new Order{ Id = 1, Name = "Karl" , Date = DateTime.Now},
            new Order{ Id = 2, Name = "Vasia", Date = DateTime.Now},
            new Order{ Id = 3, Name = "Petia", Date = DateTime.Now},
            new Order{ Id = 4, Name = "Fedia", Date = DateTime.Now},
        }.AsQueryable();
    }
}
