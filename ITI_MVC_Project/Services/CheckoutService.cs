using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckoutService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<CheckoutVM?> GetCheckoutViewAsync(string userId)
        {
            var cartItems = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .ToListAsync();

            if (!cartItems.Any()) return null;

            return new CheckoutVM
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
        }

        public async Task<(bool Success, int OrderId, string? Error)> PlaceOrderAsync(string userId, CheckoutVM model)
        {
            var cartItems = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .ToListAsync();

            if (!cartItems.Any())
                return (false, 0, "Your cart is empty.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var cartItem in cartItems)
                {
                    if (cartItem.Product == null || cartItem.Product.Stock < cartItem.Quantity)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return (false, 0, $"'{cartItem.Product?.Name}' does not have enough stock.");
                    }
                }

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = cartItems.Sum(c => (c.Product?.Price ?? 0) * c.Quantity),
                    Status = OrderStatus.Pending,
                    ShippingAddress = model.ShippingAddress,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber
                };
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                foreach (var cartItem in cartItems)
                {
                    await _unitOfWork.OrderItems.AddAsync(new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.Product!.Price
                    });

                    cartItem.Product.Stock -= cartItem.Quantity;
                    _unitOfWork.Products.Update(cartItem.Product);
                }

                var userCartItems = await _unitOfWork.CartItems.FindAsync(c => c.UserId == userId);
                foreach (var item in userCartItems)
                    _unitOfWork.CartItems.Delete(item);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return (true, order.Id, null);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return (false, 0, "An error occurred while processing your order.");
            }
        }
    }
}
