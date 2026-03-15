using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Services.Admin;

namespace ITI_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IAdminOrderService _orderService;
        public OrdersController(IAdminOrderService orderService) => _orderService = orderService;

        public async Task<IActionResult> Index()
        {
            var vm = await _orderService.GetAllOrdersAsync();
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vm = await _orderService.GetOrderDetailsAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var (success, error) = await _orderService.UpdateStatusAsync(id, status);

            if (!success)
            {
                if (error != null)
                {
                    TempData["Error"] = error;
                    return RedirectToAction(nameof(Details), new { id });
                }
                return NotFound();
            }

            TempData["Success"] = "Order status updated.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}