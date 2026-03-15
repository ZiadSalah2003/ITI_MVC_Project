using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Repositories;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index()
        {
            var orders = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var vm = new AdminOrderListVM
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
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.User)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var vm = new AdminOrderDetailsVM
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
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Order status updated.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}