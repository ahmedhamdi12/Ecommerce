using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } // Identity user

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        public string Status { get; set; } = "Pending";

        public List<OrderItem> OrderItems { get; set; }
    }
}
