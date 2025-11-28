using EzShop.Api.Models;

namespace EzShop.Api.Repository
{
    public interface IOrderRepository
    {
        Task<List<OrderHeader>?> GetOrdersAsync(string userId = "");
        Task<OrderHeader?> GetOrderByIdAsync(int orderId);
        Task<OrderHeader?> CreateOrderHeaderAsync(OrderHeader orderHeader);
        Task<List<OrderDetail>?> CreateOrderDetailAsync(List<OrderDetail> orderDetails);
    }
}
