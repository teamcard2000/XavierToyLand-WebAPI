using System.Runtime.CompilerServices;
using XavierPlayLandAPI.Filters;
using XavierPlayLandAPI.Models.Repositories;

namespace XavierPlayLandAPI.Models
{
    public class OrderDetail : IEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [QuantityCount(1)]
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Order_Subtotal { get; set; }

        // calculate the subtotal of a product in quantity
        public double CalculateSubtotal()
        {
            return Quantity * Price;
        }
    }
}
