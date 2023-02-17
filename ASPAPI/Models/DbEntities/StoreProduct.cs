using ASPAPI.Abstract.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities {
    public class StoreProduct : IEntity {
#nullable disable
        public int Id { get; set; }

        [ForeignKey(nameof(StoreId))]
        public int StoreId { get; set; }
        [JsonIgnore]
        public Store Store { get; set; }

        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

        [ForeignKey(nameof(OrderItemId))]
        public int OrderItemId { get; set; } = 0;
        [JsonIgnore]
        public OrderItem OrderItem { get; set; }


        public int StoreCount { get; set; }

        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }
        [JsonIgnore]
        public List<Store> Stores { get; set; } 

#nullable restore
    }
}
