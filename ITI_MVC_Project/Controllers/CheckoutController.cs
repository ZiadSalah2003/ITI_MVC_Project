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
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckoutController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == UserId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var vm = new CheckoutVM
            {
                CartItems = cartItems.Select(c => new CartItemVM
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product?.Name ?? "",
                    Price = c.Product?.Price ?? 0,
                    Quantity = c.Quantity,
                    ImageUrl = c.Product?.ImageUrl,
                    Stock = c.Product?.Stock ?? 0
                }).ToList(),
                TotalAmount = cartItems.Sum(c => (c.Product?.Price ?? 0) * c.Quantity)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutVM model)
        {
            var cartItems = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == UserId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            model.CartItems = cartItems.Select(c => new CartItemVM
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product?.Name ?? "",
                Price = c.Product?.Price ?? 0,
                Quantity = c.Quantity,
                ImageUrl = c.Product?.ImageUrl,
                Stock = c.Product?.Stock ?? 0
            }).ToList();
            model.TotalAmount = cartItems.Sum(c => (c.Product?.Price ?? 0) * c.Quantity);

            if (!ModelState.IsValid) return View(model);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var cartItem in cartItems)
                {
                    if (cartItem.Product == null || cartItem.Product.Stock < cartItem.Quantity)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        TempData["Error"] = $"'{cartItem.Product?.Name}' does not have enough stock.";
                        return View(model);
                    }
                }

                var order = new Order
                {
                    UserId = UserId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = model.TotalAmount,
                    Status = OrderStatus.Pending,
                    ShippingAddress = model.ShippingAddress,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber
                };
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.Product!.Price
                    };
                    await _unitOfWork.OrderItems.AddAsync(orderItem);

                    cartItem.Product.Stock -= cartItem.Quantity;
                    _unitOfWork.Products.Update(cartItem.Product);
                }

                var userCartItems = await _unitOfWork.CartItems.FindAsync(c => c.UserId == UserId);
                foreach (var item in userCartItems)
                    _unitOfWork.CartItems.Delete(item);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                TempData["Success"] = "Order placed successfully!";
                return RedirectToAction("Details", "Orders", new { id = order.Id });
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                TempData["Error"] = "An error occurred while processing your order.";
                return View(model);
            }
        }
    }
}