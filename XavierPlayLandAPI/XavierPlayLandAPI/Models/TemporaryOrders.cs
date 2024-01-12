using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using XavierPlayLandAPI.Models.Repositories;

namespace XavierPlayLandAPI.Models
{
    public static class TemporaryOrders
    {
        public static List<Order> Orders = new List<Order>
        {
            new Order { Id = 1, UserId = 1, Order_Date = DateOnly.FromDateTime(DateTime.Now), Shipping_Address = "123 Louise Street", Recepient_Phone = "123-456-7890", Payment_Method = "Mastercard", Order_Status = "Ordered"},
            new Order{ Id = 2, UserId = 2, Order_Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), Shipping_Address = "456 Louise Street", Recepient_Phone = "987-654-3210", Payment_Method = "Visa", Order_Status = "Delivered"}
        };
        public static List<OrderDetail> OrderDetails = new List<OrderDetail>
        {
            new OrderDetail { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2 },
            new OrderDetail { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1 },
            new OrderDetail { Id = 3, OrderId = 2, ProductId = 3, Quantity = 1 }
        };

        static TemporaryOrders()
        {
            foreach (var order in Orders)
            {
                var detailsForOrder = OrderDetails.Where(od => od.OrderId == order.Id).ToList();

                if (detailsForOrder.Any())
                {
                    order.OrderDetails = detailsForOrder;
                    order.Order_Total = detailsForOrder.Sum(od => od.Quantity * od.Order_Subtotal);
                }
                else
                {
                    order.Order_Total = 0;
                }
            }
        }

    }
}
