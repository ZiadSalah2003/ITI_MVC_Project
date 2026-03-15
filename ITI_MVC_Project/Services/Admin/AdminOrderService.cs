using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services.Admin
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminOrderService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<AdminOrderListVM> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return new AdminOrderListVM
            {
                Orders = orders.Select(o => new AdminOrderItemVM
                {
                    Id = o.Id,
                    CustomerName = $"{o.User?.FirstName} {o.User?.LastName}",
                    CustomerEmail = o.User?.Email ?? "",
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    ItemCount = o.OrderItems?.Count ?? 0
                })
            };
        }

        public async Task<AdminOrderDetailsVM?> GetOrderDetailsAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.User)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

            if (order == null) return null;

            return new AdminOrderDetailsVM
            {
                Id = order.Id,
                CustomerName = $"{order.User?.FirstName} {order.User?.LastName}",
                CustomerEmail = order.User?.Email ?? "",
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

        public async Task<bool> UpdateStatusAsync(int id, OrderStatus status)
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return false;

            var previousStatus = order.Status;
            order.Status = status;
            _unitOfWork.Orders.Update(order);

            if (status == OrderStatus.Cancelled && previousStatus != OrderStatus.Cancelled)
            {
                foreach (var item in order.OrderItems)
                {
                    if (item.Product != null)
                    {
                        item.Product.Stock += item.Quantity;
                        _unitOfWork.Products.Update(item.Product);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
