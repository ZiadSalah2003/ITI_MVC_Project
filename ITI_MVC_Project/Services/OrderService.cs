using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<OrderListVM> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.Orders.GetQueryable()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return new OrderListVM
            {
                Orders = orders.Select(o => new OrderSummaryVM
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status
                })
            };
        }

        public async Task<OrderDetailsVM?> GetOrderDetailsAsync(string userId, int id)
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null) return null;

            return new OrderDetailsVM
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                City = order.City,
                PhoneNumber = order.PhoneNumber,
                Items = order.OrderItems.Select(oi => new OrderItemVM
                {
                    ProductName = oi.Product?.Name ?? "",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ImageUrl = oi.Product?.ImageUrl
                })
            };
        }
    }
}
