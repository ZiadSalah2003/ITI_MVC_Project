using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITI_MVC_Project.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

        public async Task<OrderDetailsVM?> GetOrderDetailsAsync(int orderId, string userId)
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

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
                }).ToList()
            };
        }

        public async Task<int> CreateOrderAsync(string userId, CheckoutVM model)
        {
            var cartItems = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var order = new Order
            {
                UserId = userId,
                ShippingAddress = model.ShippingAddress,
                City = model.City,
                PhoneNumber = model.PhoneNumber,
                TotalAmount = cartItems.Sum(c => (c.Product?.Price ?? 0) * c.Quantity),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product?.Price ?? 0
                }).ToList()
            };

            await _unitOfWork.Orders.AddAsync(order);

            foreach (var item in cartItems)
                _unitOfWork.CartItems.Delete(item);

            await _unitOfWork.SaveChangesAsync();

            return order.Id;
        }
    }
}