using ASPAPI.Abstract.Models;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities {
    public class Store : IEntity {
#nullable disable
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        [JsonIgnore]
        public List<StoreProduct> StoreProducts { get; set; }
#nullable restore
    }
    
}
