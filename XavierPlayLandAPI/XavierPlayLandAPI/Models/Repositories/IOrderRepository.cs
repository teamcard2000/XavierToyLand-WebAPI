namespace XavierPlayLandAPI.Models.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Order? GetOrderById(int id);
        Task CreateOrder(Order order);
        Task UpdateOrder(Order updatedOrder);
        Task DeleteOrder(int id);
    }
}
