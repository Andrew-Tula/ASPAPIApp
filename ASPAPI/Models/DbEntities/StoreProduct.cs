using ASPAPI.Abstract.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities {
    public class StoreProduct : IEntity {
#nullable disable
        public int Id { get; set; }

        public int StoreCount { get; set; }

        [ForeignKey(nameof(StoreId))]
        public int StoreId { get; set; }
        [JsonIgnore]
        public Store Store { get; set; }

        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
       

        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }
#nullable restore
    }
}
