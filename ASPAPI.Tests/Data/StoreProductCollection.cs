using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class StoreProductCollection
{
    public IQueryable<StoreProduct> StoreProducts { get; }

    public StoreProductCollection()
    {
        StoreProducts = new List<StoreProduct> {
            new StoreProduct{ Id = 1, StoreCount = 8 },
            new StoreProduct{ Id = 2, StoreCount = 0 },
            new StoreProduct{ Id = 3, StoreCount = 100500 },
            new StoreProduct{ Id = 4, StoreCount = 205 },

        }.AsQueryable();
    }
}
