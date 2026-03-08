using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITI_MVC_Project.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartVM> GetCartAsync(string userId)
        {
            var items = await _unitOfWork.CartItems.GetQueryable()
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
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

        public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
        {
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
                    Quantity = quantity
                });
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var item = await _unitOfWork.CartItems.FirstOrDefaultAsync(
                c => c.Id == cartItemId && c.UserId == userId);

            if (item == null) return;

            if (quantity <= 0)
                _unitOfWork.CartItems.Delete(item);
            else
            {
                item.Quantity = quantity;
                _unitOfWork.CartItems.Update(item);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int cartItemId)
        {
            var item = await _unitOfWork.CartItems.FirstOrDefaultAsync(
                c => c.Id == cartItemId && c.UserId == userId);

            if (item != null)
            {
                _unitOfWork.CartItems.Delete(item);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var items = await _unitOfWork.CartItems.FindAsync(c => c.UserId == userId);
            foreach (var item in items)
                _unitOfWork.CartItems.Delete(item);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _unitOfWork.CartItems.CountAsync(c => c.UserId == userId);
        }
    }
}