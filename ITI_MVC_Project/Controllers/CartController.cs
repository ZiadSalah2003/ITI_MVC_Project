using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Services;
using System.Security.Claims;

namespace ITI_MVC_Project.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) => _cartService = cartService;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var vm = await _cartService.GetCartAsync(UserId);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Add() => RedirectToAction("Index");

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var error = await _cartService.AddToCartAsync(UserId, productId, quantity);
            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index", "Catalog");
            }
            TempData["Success"] = "Item added to cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, int quantity)
        {
            await _cartService.UpdateCartItemAsync(UserId, id, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _cartService.RemoveFromCartAsync(UserId, id);
            return RedirectToAction("Index");
        }
    }
}