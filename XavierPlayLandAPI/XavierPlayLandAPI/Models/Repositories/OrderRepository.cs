
namespace XavierPlayLandAPI.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IProductRepository _productRepository;

        public OrderRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrders() 
        {
            // Assuming you have a method to calculate the subtotal for each OrderDetail
            foreach (var order in TemporaryOrders.Orders)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _productRepository.GetProductById(detail.ProductId);
                    if (product != null)
                    {
                        detail.Price = product.Price.GetValueOrDefault();
                        detail.Order_Subtotal = detail.CalculateSubtotal(); // Recalculate the subtotal
                    }
                }
                order.Order_Total = order.CalculateTotal(); // Recalculate the order total
            }

            return TemporaryOrders.Orders.ToList();
        }
        
        public Order? GetOrderById (int id)
        {
            return TemporaryOrders.Orders.FirstOrDefault(o => o.Id == id);
        }

        public async Task CreateOrder(Order order)
        {
            // assign new order ID to the new order
            int newOrderId = TemporaryOrders.Orders.Any() ? TemporaryOrders.Orders.Max(o => o.Id) + 1 : 1;
            order.Id = newOrderId;

            // check if user exists
            var userExists = TemporaryUsers.Users.Any(u => u.Id == order.UserId);
            if (!userExists)
            {
                throw new ArgumentException("User does not exist.");
            }

            // Process each OrderDetail and calculate the subtotal of the product
            int newOrderDetailId = TemporaryOrders.OrderDetails.Any() ? TemporaryOrders.OrderDetails.Max(od => od.Id) + 1 : 1;
            foreach (var detail in order.OrderDetails)
            {
                // assign an new order ID to the new OrderDetail
                detail.Id = newOrderDetailId++;

                // set the OrderId for the OrderDetail
                detail.OrderId = newOrderId;

                // retrieve the product to get the price
                var product = await _productRepository.GetProductById(detail.ProductId);
                if (product != null)
                {
                    detail.Price = product.Price.GetValueOrDefault();
                    detail.Order_Subtotal = detail.CalculateSubtotal(); // Ensure CalculateSubtotal() method exists
                }
                else
                {
                    throw new ArgumentException($"Product with ID {detail.ProductId} does not exist.");
                }
            }

            // Calculate the total for the new order
            order.Order_Total = order.CalculateTotal();

            // Add the order with the calculated total and details
            TemporaryOrders.Orders.Add(order);
            TemporaryOrders.OrderDetails.AddRange(order.OrderDetails);
        }

        public async Task UpdateOrder(Order updatedOrder)
        {
            // check if user exists
            var userExists = TemporaryUsers.Users.Any(u => u.Id == updatedOrder.UserId);
            if (!userExists)
            {
                throw new ArgumentException("User does not exist.");
            }

            // Set product prices and calculate order subtotal
            foreach (var detail in updatedOrder.OrderDetails)
            {
                var product = await _productRepository.GetProductById(detail.ProductId);
                if (product != null)
                {
                    detail.Price = product.Price.GetValueOrDefault();
                    detail.Order_Subtotal = detail.CalculateSubtotal(); // Ensure CalculateSubtotal() method exists
                }
                else
                {
                    throw new ArgumentException($"Product with ID {detail.ProductId} does not exist.");
                }
            }

            var order = TemporaryOrders.Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
            if (order != null)
            {
                // Update order properties
                order.UserId = updatedOrder.UserId;
                order.Order_Date = updatedOrder.Order_Date;
                order.Shipping_Address = updatedOrder.Shipping_Address;
                order.Recepient_Phone = updatedOrder.Recepient_Phone;
                order.Payment_Method = updatedOrder.Payment_Method;
                order.Order_Status = updatedOrder.Order_Status;

                // If order details are provided, update them
                if (updatedOrder.OrderDetails != null && updatedOrder.OrderDetails.Any())
                {
                    // Remove existing details
                    TemporaryOrders.OrderDetails.RemoveAll(od => od.OrderId == updatedOrder.Id);

                    // Add new details with auto-incremented IDs
                    int maxOrderDetailId = TemporaryOrders.OrderDetails.Any() ? TemporaryOrders.OrderDetails.Max(od => od.Id) : 0;
                    foreach (var detail in updatedOrder.OrderDetails)
                    {
                        detail.Id = ++maxOrderDetailId; // Auto-increment the ID
                        detail.OrderId = updatedOrder.Id; // Ensure the OrderId is set
                        TemporaryOrders.OrderDetails.Add(detail); // Add to the static list
                    }

                    // Update the OrderDetails reference in the order object
                    order.OrderDetails = updatedOrder.OrderDetails;
                }

                // Recalculate the order total
                order.Order_Total = order.CalculateTotal();
            }
        }


        public Task DeleteOrder(int id)
        {
            var order = GetOrderById(id);
            if (order != null)
            {
                TemporaryOrders.Orders.Remove(order);
            }
            return Task.CompletedTask;
        }
    }
}
