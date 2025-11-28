using EzShop.Api.Data;
using EzShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EzShop.Api.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<OrderHeader>?> GetOrdersAsync(string userId="")
        {
           var orders=await context.OrderHeaders.Include(u=>u.OrderDetails).ThenInclude(u=>u.Product).OrderByDescending(u=>u.OrderHeaderId).ToListAsync();
            if(!string.IsNullOrEmpty(userId))
            {
                orders=orders.Where(u=>u.UserId==userId).ToList();
            }
            return orders;
        }

        public async Task<OrderHeader?> GetOrderByIdAsync(int orderId)
        {
            var order=await context.OrderHeaders.Include(u=>u.OrderDetails).ThenInclude(u=>u.Product).FirstOrDefaultAsync(u => u.OrderHeaderId == orderId);
            return order;
        }
        public async Task<OrderHeader?> CreateOrderHeaderAsync(OrderHeader orderHeader)
        {
            var order=await context.OrderHeaders.AddAsync(orderHeader);
            await context.SaveChangesAsync();
            return orderHeader;
        }
        public async Task<List<OrderDetail>?> CreateOrderDetailAsync(List<OrderDetail> orderDetails)
        {
            foreach (var orderDetail in orderDetails)
            {
                await context.OrderDetails.AddAsync(orderDetail);
            }
            await context.SaveChangesAsync();

            return orderDetails;

        }
    }
}
