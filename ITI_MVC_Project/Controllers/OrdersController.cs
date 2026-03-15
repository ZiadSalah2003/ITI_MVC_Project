using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Services;
using System.Security.Claims;

namespace ITI_MVC_Project.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) => _orderService = orderService;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var vm = await _orderService.GetUserOrdersAsync(UserId);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vm = await _orderService.GetOrderDetailsAsync(UserId, id);
            if (vm == null) return NotFound();
            return View(vm);
        }
    }
}