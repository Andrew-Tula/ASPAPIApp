using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class StoreCollection
{
    public IQueryable<Store> Stores { get; }

    public StoreCollection()
    {
        Stores = new List<Store> {
            new Store{ Id = 1, Name = "Metro", Address = "Kitaevka-5" },
            new Store{ Id = 2, Name = "SelGross", Address = "Gorelki-110" },
            new Store{ Id = 3, Name = "Globus", Address = "Levoberejny-1" },
            new Store{ Id = 4, Name = "Billa", Address = "Frunze-15" },

        }.AsQueryable();
    }
}
