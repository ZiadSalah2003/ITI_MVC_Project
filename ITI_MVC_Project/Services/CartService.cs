using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<CartVM> GetCartAsync(string userId)
        {
            var items = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .ToListAsync();

            return new CartVM
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
        }

        public async Task<string?> AddToCartAsync(string userId, int productId, int quantity)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null || !product.IsActive)
                return "Product not found.";

            var existing = await _unitOfWork.CartItems.FirstOrDefaultAsync(
                c => c.UserId == userId && c.ProductId == productId);

            if (existing != null)
            {
                existing.Quantity += quantity;
                _unitOfWork.CartItems.Update(existing);
            }
            else
            {
                await _unitOfWork.CartItems.AddAsync(new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return null;
        }

        public async Task UpdateCartItemAsync(string userId, int itemId, int quantity)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(itemId);
            if (item == null || item.UserId != userId) return;

            if (quantity <= 0)
            {
                item.IsDeleted = true;
                _unitOfWork.CartItems.Update(item);
            }
            else
            {
                item.Quantity = quantity;
                _unitOfWork.CartItems.Update(item);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int itemId)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(itemId);
            if (item != null && item.UserId == userId)
            {
                item.IsDeleted = true;
                _unitOfWork.CartItems.Update(item);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
