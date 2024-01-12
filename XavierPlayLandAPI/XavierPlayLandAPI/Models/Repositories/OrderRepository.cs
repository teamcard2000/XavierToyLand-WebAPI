
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
                    if (product.Quantity <= 0)
                    {
                        throw new ArgumentException("This product is out of stock.");
                    }
                    else if (detail.Quantity > product.Quantity)
                    {
                        throw new ArgumentException($"The requested quantity for Product ID {detail.ProductId} exceeds the available stock.");
                    }
                    else
                    {
                        product.Quantity -= detail.Quantity;
                        if (product.Quantity <= 0 && product.isAvailable == true)
                        {
                            product.isAvailable = false;
                        }

                        detail.Price = product.Price.GetValueOrDefault();
                        detail.Order_Subtotal = detail.CalculateSubtotal();

                        // set order status to ordered when they order the product
                        order.Order_Status = "Ordered";
                    }
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
            // Check if the user exists
            var userExists = TemporaryUsers.Users.Any(u => u.Id == updatedOrder.UserId);
            if (!userExists)
            {
                throw new ArgumentException("User does not exist.");
            }

            // Retrieve the existing order and update it
            var order = TemporaryOrders.Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
            if (order != null)
            {
                order.UserId = updatedOrder.UserId;
                order.Order_Date = updatedOrder.Order_Date;
                order.Shipping_Address = updatedOrder.Shipping_Address;
                order.Recepient_Phone = updatedOrder.Recepient_Phone;
                order.Payment_Method = updatedOrder.Payment_Method;
                order.Order_Status = updatedOrder.Order_Status;

                if (updatedOrder.OrderDetails != null && updatedOrder.OrderDetails.Any())
                {
                    // Validate and set product prices and calculate order subtotal
                    foreach (var detail in updatedOrder.OrderDetails)
                    {
                        var product = await _productRepository.GetProductById(detail.ProductId);
                        if (product != null)
                        {
                            // re-calculate the subtotal
                            detail.Price = product.Price.GetValueOrDefault();
                            detail.Order_Subtotal = detail.CalculateSubtotal();
                        }
                    }

                    // Remove existing details
                    TemporaryOrders.OrderDetails.RemoveAll(od => od.OrderId == updatedOrder.Id);

                    // Add new details with auto-incremented IDs
                    int maxOrderDetailId = TemporaryOrders.OrderDetails.Any() ? TemporaryOrders.OrderDetails.Max(od => od.Id) : 0;
                    foreach (var detail in updatedOrder.OrderDetails)
                    {
                        detail.Id = ++maxOrderDetailId;
                        detail.OrderId = updatedOrder.Id;
                        TemporaryOrders.OrderDetails.Add(detail);
                    }

                    // Update the OrderDetails reference in the order object
                    order.OrderDetails = updatedOrder.OrderDetails;

                    // Recalculate the order total
                    order.Order_Total = order.CalculateTotal();
                }
            }
            else
            {
                throw new ArgumentException("Order does not exist.");
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
