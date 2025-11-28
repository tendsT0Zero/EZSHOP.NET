using EzShop.Api.Data;
using EzShop.Api.DTOs;
using EzShop.Api.Models;
using EzShop.Api.Repository;
using EzShop.Api.Static_Details;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly AppDbContext context;
        public OrderController(IOrderRepository orderRepository,AppDbContext context)
        {
            this.orderRepository = orderRepository;
            this.context = context;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders(string userId="")
        {
            var orders = await orderRepository.GetOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("orders/{orderId:int}")]
        public async Task<IActionResult> GetOrders([FromRoute] int orderId)
        {
            var orders = await orderRepository.GetOrderByIdAsync(orderId);
            return Ok(orders);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OrderHeaderCreateDTO orderHeaderDto)
        {
            // 1. Create OrderHeader
            var orderHeaderDomain = new OrderHeader
            {
                PickUpEmail = orderHeaderDto.PickUpEmail,
                PickUpName = orderHeaderDto.PickUpName,
                PickUpPhoneNumber = orderHeaderDto.PickUpPhoneNumber,
                OrderDate = DateTime.Now,
                OrderStatus = SD.status_confirmed,
                OrderTotal = orderHeaderDto.OrderTotal,
                TotalItems = orderHeaderDto.TotalItems,
                UserId = orderHeaderDto.UserId
            };

            await context.OrderHeaders.AddAsync(orderHeaderDomain);
            await context.SaveChangesAsync();

            foreach (var dto in orderHeaderDto.OrderDetailCreateDTOs)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = dto.ProductId,
                    OrderHeaderId = orderHeaderDomain.OrderHeaderId,  
                    Quantity = dto.Quantity,
                    ProductName = dto.ProductName,
                    Price = dto.Price
                };

                await context.OrderDetails.AddAsync(orderDetail);
            }
            await context.SaveChangesAsync();

            return Ok("Order Placed Successfully");
        }


        [HttpPut("{orderId:int}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int orderId, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var existingOrder = await context.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderId);

                if (existingOrder == null)
                {
                    return NotFound("Order not found with that id.");
                }

                if (!string.IsNullOrWhiteSpace(orderHeaderUpdateDTO.PickUpName)) existingOrder.PickUpName = orderHeaderUpdateDTO.PickUpName;
                if (!string.IsNullOrWhiteSpace(orderHeaderUpdateDTO.PickUpPhoneNumber)) existingOrder.PickUpPhoneNumber = orderHeaderUpdateDTO.PickUpPhoneNumber;
                if (!string.IsNullOrWhiteSpace(orderHeaderUpdateDTO.PickUpEmail)) existingOrder.PickUpEmail = orderHeaderUpdateDTO.PickUpEmail;
                if (!string.IsNullOrWhiteSpace(orderHeaderUpdateDTO.OrderStatus))
                {
                    if (existingOrder.OrderStatus.Equals(SD.status_confirmed, StringComparison.InvariantCultureIgnoreCase) 
                        && orderHeaderUpdateDTO.OrderStatus.Equals(SD.status_readyForPickup, StringComparison.InvariantCultureIgnoreCase))
                    {
                        existingOrder.OrderStatus = SD.status_readyForPickup;
                    }

                    if (existingOrder.OrderStatus.Equals(SD.status_readyForPickup, StringComparison.InvariantCultureIgnoreCase)
                        && orderHeaderUpdateDTO.OrderStatus.Equals(SD.status_completed, StringComparison.InvariantCultureIgnoreCase))
                    {
                        existingOrder.OrderStatus = SD.status_completed;
                    }
                    if (orderHeaderUpdateDTO.OrderStatus.Equals(SD.status_cancel, StringComparison.InvariantCultureIgnoreCase))
                    {
                        existingOrder.OrderStatus = SD.status_cancel;
                    }
                }
                await context.SaveChangesAsync();
                return Ok("Updated  successfully!");

            }
            return BadRequest("Something wen wrong.TryAgain");
        }

    }
}
