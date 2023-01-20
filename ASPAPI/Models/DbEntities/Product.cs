using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities {
    public class Product {
#nullable disable
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }
#nullable restore
    }
}


