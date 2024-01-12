using System.ComponentModel.DataAnnotations;
using XavierPlayLandAPI.Filters;
using XavierPlayLandAPI.Models.Repositories;

namespace XavierPlayLandAPI.Models
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }
        public DateOnly Order_Date { get; set; }
        [Required(ErrorMessage = "Shipping Address is required.")]
        public string? Shipping_Address { get; set; }
        [Required(ErrorMessage = "Recepient Phone Number is required.")]
        public string? Recepient_Phone { get; set; }
        [PaymentMethod]
        public string? Payment_Method { get; set; }
        [OrderStatus]
        public string? Order_Status { get; set; }
        public double Order_Total { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public double CalculateTotal()
        {
            double total = 0;
            foreach (var detail in OrderDetails)
            {
                // Assume that each OrderDetail now has a Price property set when it's created
                total += detail.Quantity * detail.Order_Subtotal;
            }
            return total;
        }
    }
}
