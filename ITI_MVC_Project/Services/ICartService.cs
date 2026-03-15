using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface ICartService
    {
        Task<CartVM> GetCartAsync(string userId);
        Task<string?> AddToCartAsync(string userId, int productId, int quantity);
        Task UpdateCartItemAsync(string userId, int itemId, int quantity);
        Task RemoveFromCartAsync(string userId, int itemId);
    }
}
