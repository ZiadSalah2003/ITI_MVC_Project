using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface ICartService
    {
        Task<CartVM> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId, int quantity = 1);
        Task UpdateQuantityAsync(string userId, int cartItemId, int quantity);
        Task RemoveFromCartAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
        Task<int> GetCartCountAsync(string userId);
    }
}