using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Repositories;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using System.Security.Claims;

namespace ITI_MVC_Project.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var items = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == UserId)
                .ToListAsync();

            var vm = new CartVM
            {
                Items = items.Select(c => new CartItemVM
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product?.Name ?? "",
                    Price = c.Product?.Price ?? 0,
                    Quantity = c.Quantity,
                    ImageUrl = c.Product?.ImageUrl,
                    Stock = c.Product?.Stock ?? 0
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null || !product.IsActive)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index", "Catalog");
            }

            var existing = await _unitOfWork.CartItems.FirstOrDefaultAsync(
                c => c.UserId == UserId && c.ProductId == productId);

            if (existing != null)
            {
                existing.Quantity += quantity;
                _unitOfWork.CartItems.Update(existing);
            }
            else
            {
                await _unitOfWork.CartItems.AddAsync(new CartItem
                {
                    UserId = UserId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync();
            TempData["Success"] = "Item added to cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int quantity)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (item == null || item.UserId != UserId)
                return NotFound();

            if (quantity <= 0)
            {
                _unitOfWork.CartItems.Delete(item);
            }
            else
            {
                item.Quantity = quantity;
                _unitOfWork.CartItems.Update(item);
            }
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (item != null && item.UserId == UserId)
            {
                _unitOfWork.CartItems.Delete(item);
                await _unitOfWork.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}