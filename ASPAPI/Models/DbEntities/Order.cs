using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities
{
    public class Order
    {
#nullable disable
        public int Id { get; set; }     
        public string Name { get; set; }    
        public DateTime Date { get; set; }

        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }
#nullable restore
    }
}
