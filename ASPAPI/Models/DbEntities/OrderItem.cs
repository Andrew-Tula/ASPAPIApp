using ASPAPI.Abstract.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities 
{
    public class OrderItem: IEntity {
#nullable disable
        public int Id { get; set; }

        public int ProductCount { get; set; }

        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

       

        [ForeignKey(nameof(OrderId))]
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
#nullable restore
    }
}
