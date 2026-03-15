using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Services;
using System.Security.Claims;

namespace ITI_MVC_Project.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutController(ICheckoutService checkoutService) => _checkoutService = checkoutService;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _checkoutService.GetCheckoutViewAsync(UserId);
            if (vm == null)
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutVM model)
        {
            if (!ModelState.IsValid)
            {
                var refreshed = await _checkoutService.GetCheckoutViewAsync(UserId);
                if (refreshed == null)
                {
                    TempData["Error"] = "Your cart is empty.";
                    return RedirectToAction("Index", "Cart");
                }
                model.CartItems = refreshed.CartItems;
                model.TotalAmount = refreshed.TotalAmount;
                return View(model);
            }

            var (success, orderId, error) = await _checkoutService.PlaceOrderAsync(UserId, model);
            if (!success)
            {
                if (error == "Your cart is empty.")
                {
                    TempData["Error"] = error;
                    return RedirectToAction("Index", "Cart");
                }
                var refreshed = await _checkoutService.GetCheckoutViewAsync(UserId);
                if (refreshed != null)
                {
                    model.CartItems = refreshed.CartItems;
                    model.TotalAmount = refreshed.TotalAmount;
                }
                TempData["Error"] = error;
                return View(model);
            }

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("Details", "Orders", new { id = orderId });
        }
    }
}